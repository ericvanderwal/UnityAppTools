using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;


namespace dgd
{
    public class RunCSV : MonoBehaviour
    {

        public string myPath;

        [Button]
        public void Run()
        {
            FetchCSVData();
        }
        
        /// <summary>
        /// Fetch csv data and parse
        /// </summary>
        private void FetchCSVData() {
	
            List<Dictionary<string,object>> data = CSVReader.Read (myPath);
		
            for(var i=0; i < data.Count; i++) {
                print ("name " + data[i]["name"] + " " +
                       "age " + data[i]["age"] + " " +
                       "speed " + data[i]["speed"] + " " +
                       "desc " + data[i]["description"]);
            }
	
        }

        /// <summary>
        /// Check path exists
        /// </summary>
        /// <returns></returns>
        [Button]
        public bool CheckPath()
        {
            var exists = File.Exists(myPath);
            Debug.Log(exists ? "File exists" : "File does not exists");
            return exists;
        }
       
    }
}