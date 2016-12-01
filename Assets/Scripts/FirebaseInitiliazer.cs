using Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class FirebaseInitiliazer
    {
        private const string TAG = "FirebaseInitializer";

        public FirebaseInitiliazer()
        {
            Firebase.DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

            dependencyStatus = FirebaseApp.CheckDependencies();
            if (dependencyStatus != DependencyStatus.Available)
            {
                FirebaseApp.FixDependenciesAsync().ContinueWith(task =>
                {
                    dependencyStatus = FirebaseApp.CheckDependencies();
                    if (!(dependencyStatus == DependencyStatus.Available))
                    {
                        Debug.LogError(
                           "Could not resolve all Firebase dependencies: " + dependencyStatus);
                    }

                });
            }

        }
    }
}
