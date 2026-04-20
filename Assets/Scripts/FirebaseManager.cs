using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private FirebaseFirestore db;
    private string playerId;

    public int totalCoins = 0;
    public bool level1Completed = false;
    public bool level2Completed = false;

    public bool IsFirebaseReady { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!PlayerPrefs.HasKey("playerId"))
        {
            PlayerPrefs.SetString("playerId", System.Guid.NewGuid().ToString());
            PlayerPrefs.Save();
        }

        playerId = PlayerPrefs.GetString("playerId");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            DependencyStatus status = task.Result;

            if (status == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                IsFirebaseReady = true;
                Debug.Log("Firebase listo.");
                LoadProgress();
            }
            else
            {
                Debug.LogError("No se pudieron resolver dependencias de Firebase: " + status);
            }
        });
    }

    public void SaveProgress()
    {
        if (!IsFirebaseReady || db == null)
        {
            Debug.LogWarning("Firebase aún no está listo para guardar.");
            return;
        }

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "totalCoins", totalCoins },
            { "level1Completed", level1Completed },
            { "level2Completed", level2Completed }
        };

        db.Collection("players").Document(playerId).SetAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log("Progreso guardado correctamente.");
            }
            else
            {
                Debug.LogError("Error al guardar progreso: " + task.Exception);
            }
        });
    }

    public void LoadProgress()
    {
        if (!IsFirebaseReady || db == null)
        {
            Debug.LogWarning("Firebase aún no está listo para cargar.");
            return;
        }

        db.Collection("players").Document(playerId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DocumentSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    if (snapshot.ContainsField("totalCoins"))
                        totalCoins = snapshot.GetValue<int>("totalCoins");

                    if (snapshot.ContainsField("level1Completed"))
                        level1Completed = snapshot.GetValue<bool>("level1Completed");

                    if (snapshot.ContainsField("level2Completed"))
                        level2Completed = snapshot.GetValue<bool>("level2Completed");

                    Debug.Log("Progreso cargado correctamente.");
                }
                else
                {
                    Debug.Log("No hay progreso previo. Se crearán datos nuevos.");
                    SaveProgress();
                }
            }
            else
            {
                Debug.LogError("Error al cargar progreso: " + task.Exception);
            }
        });
    }
}