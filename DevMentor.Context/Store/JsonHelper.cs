using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DevMentor.Context.Store
{
    internal class JsonHelper
    {

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(String pstring)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pstring);
            return byteArray;
        }

        ///<summary>
        /// Method to convert a custom Object to JSON string
        /// </summary>
        /// <param name="pObject">Object that is to be serialized to JSON</param>
        /// <param name="objectType"></param>
        /// <returns>JSON string</returns>
        public static String SerializeObject(Object pobject, Type objectType)
        {
            try
            {

                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                settings.DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("o");


                var jsonSerializer = new DataContractJsonSerializer(objectType, settings);
                //jsonSerializer.DateTimeFormat.

                string returnValue = "";
                using (var memoryStream = new MemoryStream())
                {
                    using (var xmlWriter = JsonReaderWriterFactory.CreateJsonWriter(memoryStream))
                    {
                        jsonSerializer.WriteObject(xmlWriter, pobject);
                        xmlWriter.Flush();
                        returnValue = Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                    }
                }
                return returnValue;

            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// Method to reconstruct an Object from JSON string
        /// </summary>
        /// <param name="pXmlizedString"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static Object DeserializeObject(String pstring, Type objectType)
        {
            if (string.IsNullOrEmpty(pstring))
                return Activator.CreateInstance(objectType);

            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            settings.DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("o");


            object returnValue;
            using (var memoryStream = new MemoryStream())
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(pstring);
                memoryStream.Write(jsonBytes, 0, jsonBytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(memoryStream,
                                          Encoding.UTF8,
                                          XmlDictionaryReaderQuotas.Max, null))
                {
                    var serializer = new DataContractJsonSerializer(objectType, settings);
                    returnValue = serializer.ReadObject(jsonReader);

                }
            }
            return returnValue;

        }

    }
}
