//Copyright 2019 Google LLC
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//https://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Firebase.Auth;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UploadGameData : MonoBehaviour
{
 //   [SerializeField] private GameOverPanel _gameOverPanel;

    // matching the rules, {0} is the user id
    [SerializeField, Tooltip("Wherse to store the score. {0} will be replaced by the UserID")]
    private string _finalScorePath = "/AllUser/{0}/ExerciseData.csv";

    public UnityEvent OnUploadStarted = new UnityEvent();
    public UploadGameDataCompleteEvent OnUploadComplete = new UploadGameDataCompleteEvent();
    public UploadGameDataFailedEvent OnUploadFailed = new UploadGameDataFailedEvent();

    private Coroutine _uploadCoroutine;
    public Image startimg;
    public TextAsset csvFile;
    private void Reset()
    {
     //   _gameOverPanel = FindObjectOfType<GameOverPanel>();
    }

    private void Start()
    {
       
    }

    public void Trigger()
    {
        if (_uploadCoroutine == null)
        {
            _uploadCoroutine = StartCoroutine(UploadData());
        }
    }

    private IEnumerator UploadData()
    {
        OnUploadStarted.Invoke();
      //  var deathData = _gameOverPanel.DeathData;
        //if (deathData == null)
        //{
        //    HandleException(
        //        FailureReason.NoDeathData,
        //        new Exception("No death data, this should only happen in editor"));
        //    yield break;
        //}

        // TODO: abstract
        var auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser == null)
        {
            var signInTask = auth.SignInAnonymouslyAsync();
            yield return new WaitUntil(() => signInTask.IsCompleted);

            if (signInTask.Exception != null)
            {
                HandleException(FailureReason.SignInFailed, signInTask.Exception);
                yield break;
            }
        }

        // //we're logged in. Yay!
        var storage = FirebaseStorage.DefaultInstance;
        var finalScoreReference = storage.GetReference(string.Format(_finalScorePath, auth.CurrentUser.UserId));

        var metadata = new MetadataChange
        {
            ContentType = "File/csv",
            CustomMetadata = new Dictionary<string, string>()
            {
                //{"duration", deathData.Duration.ToString(CultureInfo.InvariantCulture)},
                //{"death_reason", deathData.DeathReason.ToString()},
                //{"death_reason_string", deathData.DeathReasonString},
                //{"final_score", deathData.FinalScore.ToString()}
            }
        };
        
        var uploadTask = finalScoreReference.PutBytesAsync(csvFile.bytes, metadata);
        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if (uploadTask.Exception != null)
        {
            HandleException(FailureReason.UploadFailed, uploadTask.Exception);
            yield break;
        }

        OnUploadComplete.Invoke(finalScoreReference);
        _uploadCoroutine = null;
        Debug.Log($"Data Uploaded to {finalScoreReference.Path}");
    }

    private void HandleException(FailureReason reason, Exception exception)
    {
        OnUploadFailed.Invoke(reason, exception);
        _uploadCoroutine = null;
        Debug.LogWarning($"Failed with {reason} because {exception}");
    }

    public enum FailureReason
    {
        None,
        SignInFailed,
        UploadFailed,
        NoDeathData
    }

    [Serializable]
    public class UploadGameDataFailedEvent : UnityEvent<FailureReason, Exception>
    {
    }

    [Serializable]
    public class UploadGameDataCompleteEvent : UnityEvent<StorageReference>
    {

    }
}