using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Threading;
using UnityEngine.Video;
using Random = System.Random;


public class RayCasting : MonoBehaviour
{
    public Material skyboxBlack;

    private Color _color;
    private bool isDone = false;

    // erste Zeile der text Datei nicht verwenden, da dort die Spaltenbeschreibungen stehen
    private bool _ersteZeile = true;

    /// <summary>
    /// originVector, (normalized) directionVector, stimuli number, timestamp of recording
    /// </summary>
    private Tuple<Vector3, Vector3, string, string> _originDirectionVectorTuple;
    Dictionary<string, List<Tuple<Vector3, Vector3, string, string>>> probandDictionary = new Dictionary<string, List<Tuple<Vector3, Vector3, string, string>>>();
    Dictionary<string, Tuple<FileInfo, FileInfo>> probandEyeTrackingPfadDictionary = new Dictionary<string, Tuple<FileInfo, FileInfo>>();

    private string[] chosenStimulus;
    private string _folderPathProbanden = "";
    private string chosenStimulusName = "";
    private bool _is360Stimuli = false;
    private VideoPlayer _videoPlayer;
    private GameObject _imageCube;
    private GameObject _sphere360;
    private List<KeyValuePair<string, Tuple<string, string, string, string>>> _angleKeyValuePairsList = new List<KeyValuePair<string, Tuple<string, string, string, string>>>();

    private RaycastHit _hit;

    void Start()
    {
        // Umgebung auf schwarz setzen
        //SetEnvironmentBlack();


        // damit es einmalig durchlaufen wird
        if (!isDone)
        {
            // stimulus Name, Breite und Höhe auslesen
            SetUnityEnvironmentToCorrespondingChosenStimulus();

            GetFolderPathProbanden();

            // alle Tracking Daten aller Probanden extrahieren
            DirectoryInfo d = new DirectoryInfo(_folderPathProbanden);
            DirectoryInfo[] dFileInfos = d.GetDirectories();

            FileInfo[] gazeFileInfos;
            FileInfo[] angleFileInfos;

            foreach (var directoryInfo in dFileInfos)
            {
                gazeFileInfos = directoryInfo.GetFiles("player-log_et-gaze.txt");
                angleFileInfos = directoryInfo.GetFiles("et-log.txt");

                probandEyeTrackingPfadDictionary.Add(directoryInfo.Name, new Tuple<FileInfo, FileInfo>(gazeFileInfos.FirstOrDefault(), angleFileInfos.FirstOrDefault()));
            }

            foreach (var kvPairProbandFileInfo in probandEyeTrackingPfadDictionary)
            {
                _ersteZeile = true;
                var firstRowAngles = true;

                var gazeFileName = kvPairProbandFileInfo.Value.Item1.ToString();
                var angleFileName = kvPairProbandFileInfo.Value.Item2.ToString(); // TODO: hier weiter machen ... TODO: euler umrechnen in normale Angels 
                var probandName = kvPairProbandFileInfo.Key;

                using (StreamReader etDataReader = new StreamReader(Path.Combine(_folderPathProbanden, gazeFileName)))
                    using (StreamReader angleDataReader = new StreamReader(Path.Combine(_folderPathProbanden, angleFileName)))
                {
                    var eyeTrackingContent = from line in ReadFile(etDataReader, "\t")
                        select new
                        {
                            stimuli = line[0],
                            type = line[1],
                            dimension = line[2],
                            path = line[3],
                            timestamp = line[4],
                            Origin_x = line[5],
                            Origin_y = line[6],
                            Origin_z = line[7],
                            Direction_x = line[8],
                            Direction_y = line[9],
                            Direction_z = line[10],
                            Proband = probandName,
                        };

                    var angleContent = from line in ReadFile(angleDataReader, "\t")
                        select new
                        {
                            stimuli = line[0],
                            CameraX = line[1],
                            CameraY = line[2],
                            CameraZ = line[3],
                            CameraW = line[4],
                        };

                    foreach (var angle in angleContent)
                    {
                        if (firstRowAngles)
                        {
                            firstRowAngles = false;
                            continue;
                        }

                        var keyValuePair = new KeyValuePair<string, Tuple< string, string, string, string>> (angle.stimuli, new Tuple<string, string, string, string>(angle.CameraX, angle.CameraY, angle.CameraW, angle.CameraZ));

                        _angleKeyValuePairsList.Add(keyValuePair);
                    }

                    foreach (var singleEyeTrackingDataPoint in eyeTrackingContent)
                    {
                        var currentStimulusName = Path.GetFileNameWithoutExtension(singleEyeTrackingDataPoint.path);

                        // aktueller Stimulus ist NICHT der vom Nutzer gewählte Stimulus
                        if (!chosenStimulusName.Equals(currentStimulusName))
                        {
                            continue;
                        }

                        if (!_ersteZeile)
                        {
                            var originVector = new Vector3(float.Parse(singleEyeTrackingDataPoint.Origin_x), float.Parse(singleEyeTrackingDataPoint.Origin_y), float.Parse(singleEyeTrackingDataPoint.Origin_z));
                            var directionVector = new Vector3(float.Parse(singleEyeTrackingDataPoint.Direction_x), float.Parse(singleEyeTrackingDataPoint.Direction_y), float.Parse(singleEyeTrackingDataPoint.Direction_z));

                            _originDirectionVectorTuple = new Tuple<Vector3, Vector3, string, string>(originVector, directionVector, singleEyeTrackingDataPoint.stimuli, singleEyeTrackingDataPoint.timestamp);

                            // try: Proband in der Liste neu anlegen, inklusive erstem Eintrag, catch: wenn Proband schon existent, dann Informationen in die zugehörige Liste hinzufügen
                            try
                            {
                                probandDictionary.Add(singleEyeTrackingDataPoint.Proband, new List<Tuple<Vector3, Vector3, string, string>> { _originDirectionVectorTuple });
                            }
                            catch (ArgumentException)
                            {
                                probandDictionary[singleEyeTrackingDataPoint.Proband].Add(_originDirectionVectorTuple);
                            }
                        }

                        _ersteZeile = false;
                    }
                }
            }

            if (_is360Stimuli)
            {
                StartCoroutine(ShowVideoRays());
            }
            else
            {
                ShowPictureRays();
            }

            isDone = true;
        }
    }

    private void GetFolderPathProbanden()
    {
        using (var probandReader = new StreamReader(Path.Combine(
            Directory.GetParent(Application.dataPath).FullName, "z_Extra_Information", "ProbandenPath.txt")))
        {
            _folderPathProbanden = probandReader.ReadLine();
        }
    }

    private void SetUnityEnvironmentToCorrespondingChosenStimulus()
    {
        using (var chosenStimulusReader = new StreamReader(Path.Combine(Directory.GetParent(Application.dataPath).FullName,
            "z_Extra_Information", "chosenStimulus.txt")))
        {
            chosenStimulus = chosenStimulusReader.ReadLine().Split("\t".ToCharArray());

            var path = chosenStimulus[0];
            var stimulusType = chosenStimulus[3]; // picture or video
            chosenStimulusName = chosenStimulus[4];

            _imageCube = GameObject.FindGameObjectWithTag("imageCube"); // Projektions-Anzeige für Bilder
            _sphere360 = GameObject.FindGameObjectWithTag("sphere360degree"); // Projektions-Anzeige für 360 Stimuli

            // TODO: anpassen: Datei ist kein 360 Grad Stimulus
            if (stimulusType.Equals("Datei ist ein Bild"))
            {
                var pictureWidth = Int32.Parse(chosenStimulus[1]);
                var pictureHeight = Int32.Parse(chosenStimulus[2]);

                // 2D Stimulus anzeigen und sphere (für 3D) ausschalten
                _imageCube.SetActive(true);
                _sphere360.SetActive(false);
                Texture2D tex2D = Resources.Load(@"Experiment/" + path) as Texture2D; // NOT full path here
                _imageCube.GetComponent<MeshRenderer>().material.mainTexture = tex2D;

                _is360Stimuli = false;

                // _imageCube auf Größe anpassen, welche auch im Programm des EyeTracking drin war
                if (pictureWidth >= 1400 && pictureHeight >= 500)
                {
                    _imageCube.transform.localScale = new Vector3(4.5f, 2.5f, 0.133f);
                }
                else
                {
                    _imageCube.transform.localScale = new Vector3(0.5f, 1.5f, 0.133f);
                }
            }
            else
            {
                // Video Player für die Kugel (sphere), um dort das Video auf der Innenseite abzuspielen
                _videoPlayer = _sphere360.AddComponent<UnityEngine.Video.VideoPlayer>();
                _videoPlayer.url = path; // full path here
                _videoPlayer.Prepare();

                // 2D Stimulus ausschalten und sphere (für 3D) anzeigen
                _sphere360.SetActive(true);
                _imageCube.SetActive(false);
                _is360Stimuli = true;
            }
        }
    }

    private void ShowPictureRays()
    {
        using (StreamWriter staticStimulusWriter = new StreamWriter(Path.Combine(
            Directory.GetParent(Application.dataPath).FullName, "z_Results", "static_result.txt")))
        {
            foreach (var probandKeyValuePair in probandDictionary)
            {
                var probandGazeRayTuplesList = probandKeyValuePair.Value;
                var probandName = probandKeyValuePair.Key;

                // eine bestimmte Farbe für einen bestimmten Probanden (random)
                _color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                foreach (var gazeRayTuple in probandGazeRayTuplesList)
                {
                    // Does the ray intersect any objects excluding the player layer
                    if (Physics.Raycast(gazeRayTuple.Item1, gazeRayTuple.Item2, out _hit, Mathf.Infinity))
                    {
                        // Infos anzeigen und schreiben
                        Debug.DrawRay(gazeRayTuple.Item1, gazeRayTuple.Item2 * _hit.distance, _color, 50, true);
                        staticStimulusWriter.WriteLine($"{gazeRayTuple.Item3}\t{probandName}\t{_hit.point}");
                    }
                }
            }
        }
    }

    IEnumerator ShowVideoRays()
    {
        // für jeden Probanden die Liste der GazeRays durchlaufen
        using (StreamWriter nonStaticStimulusWriter = new StreamWriter(Path.Combine(
            Directory.GetParent(Application.dataPath).FullName, "z_Results", "non_static_result.txt")))
        {
            foreach (var probandKeyValuePair in probandDictionary)
            {
                var probandGazeRayTuplesList = probandKeyValuePair.Value;
                var probandName = probandKeyValuePair.Key;

                // eine bestimmte Farbe für einen bestimmten Probanden (random)
                _color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                bool first = true;
                string firstFrame_Time = "";

                var milliseconds = 1000f;
                var messungTimestamp = 0.0f;

                var stimuliID = probandGazeRayTuplesList.FirstOrDefault().Item3;

                // Rotation des Videos anpassen
                foreach (var angleKeyValuePair in _angleKeyValuePairsList)
                {
                    // key is the stimuliID
                    if (angleKeyValuePair.Key.Equals(stimuliID))
                    {
                        var CameraX = float.Parse(angleKeyValuePair.Value.Item1);
                        var CameraY = float.Parse(angleKeyValuePair.Value.Item2);
                        var CameraZ = float.Parse(angleKeyValuePair.Value.Item3);
                        var CameraW = float.Parse(angleKeyValuePair.Value.Item4);

                        _sphere360.transform.rotation = new Quaternion(CameraX, CameraY, CameraZ, CameraW);

                        // y um 180 Grad drehen, da invertierte Kugel (sonst spiegelverkehrtes Anzeigebild)
                        _sphere360.transform.Rotate(0, 180, 0);
                        break;
                    }
                }


                _videoPlayer.Play();

                // wait for video playback
                while (_videoPlayer.frame == -1)
                {
                    yield return null;
                }


                // iterate over all origin and direction Vectors
                foreach (var gazeRayTuple in probandGazeRayTuplesList)
                {
                    if (first)
                    {
                        firstFrame_Time = gazeRayTuple.Item4;
                        first = false;
                    }
                    else
                    {
                        messungTimestamp = (float.Parse(gazeRayTuple.Item4) - float.Parse(firstFrame_Time)) / milliseconds;
                    }


                    while (messungTimestamp >= _videoPlayer.time && _videoPlayer.isPlaying)
                    {
                        yield return null;
                    }


                    // Kugel hat Radius 20, daher (gazeRayTuple.Item2 * 20) || Item2 is normalisierter Richtungsvektor
                    Debug.DrawRay(new Vector3(0f, 0f, 0f), gazeRayTuple.Item2 * 20, _color, 0.1f, true);
                    nonStaticStimulusWriter.WriteLine($"{gazeRayTuple.Item3}\t{probandName}\t{gazeRayTuple.Item2 * 20}\t{messungTimestamp}");
                }

                _videoPlayer.Stop();
            }
        }
    }



    // Hilfsfunktionen
    void SetEnvironmentBlack()
    {
        skyboxBlack = Resources.Load<Material>("Skyboxes/skybox_black");
        RenderSettings.skybox = skyboxBlack;
        DynamicGI.UpdateEnvironment();
    }

    private static IEnumerable<string[]> ReadFile(StreamReader reader, string delimiter)
    {
        while (reader.EndOfStream == false)
        {
            yield return reader.ReadLine().Split(delimiter.ToCharArray());
        }
    }
}