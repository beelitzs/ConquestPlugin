using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ConquestPlugin
{
    using VRage.FileSystem;
   
    [Serializable]
    public class PluginSettings
    {
        #region Private Fields
        private static PluginSettings _instance;
        private int _DifficultyMod;
        private string _FPIncomeRate;
        private float _ReqBroadcastDistance;
        private bool _T2Enabled;
        private bool _T3Enabled;
        #endregion
        #region Static Properties
        public static PluginSettings Instance
        {
            get { return _instance ?? (_instance = new PluginSettings());}
        }
        #endregion
        #region Prorerties
        public int Difficutlymod
        {
            get { return _DifficultyMod; }
            set
            {
                _DifficultyMod = value;
                Save();
            }
        }
        
        public string FPIncomeRate
        {
            get { return _FPIncomeRate; }
            set
            {
                _FPIncomeRate = value;
                Save();
            }
        }
        public float ReqBroadcastDistance
        {
            get { return _ReqBroadcastDistance; }
            set
            {
                _ReqBroadcastDistance = value;
                Save();
            }
        }
        public bool T2Enabled
        {
            get { return _T2Enabled; }
            set
            {
                _T2Enabled = value;
                Save();
            }
        }
        public bool T3Enabled
        {
            get { return _T3Enabled; }
            set
            {
                _T3Enabled = value;
                Save();
            }
        }
        #endregion
        #region Loading and Saving
        public void Load()
        {
           try
           {
               lock(this)
               {
                   string fileName = Conquest.PluginPath + "Conquest-Settings.xml";
                   if (File.Exists(fileName))
                   {
                       using (StreamReader reader = new StreamReader(fileName))
                       {
                           XmlSerializer x = new XmlSerializer(typeof(PluginSettings));
                           PluginSettings settings = (PluginSettings)x.Deserialize(reader);
                           reader.Close();

                           _instance = settings;
                       }
                   }
               }
           }
            catch(Exception ex)
           {
               Conquest.Log.Error(ex);
           }
        }

        public void Save()
        {
            try
            {
                lock(this)
                {
                    string fileName = Conquest.PluginPath + "Conquest-Settings.xml";
                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        XmlSerializer x = new XmlSerializer(typeof(PluginSettings));
                        x.Serialize(writer, _instance);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Conquest.Log.Error(ex);
            }
        }
        #endregion
    }
}
