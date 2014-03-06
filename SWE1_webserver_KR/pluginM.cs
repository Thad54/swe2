using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Reflection;

namespace SWE1_webserver_KR
{
    class pluginM
    {
        private ArrayList plugins;
        private Type type;

        public pluginM(){

            plugins = new ArrayList();
            type = typeof(SWE1_webserver_KR.iPlugin);

        }

       public void loadPlugins()
        {
            foreach(string dll in Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "*.dll")){
                    Assembly Aplugin = Assembly.LoadFrom(dll);

                    foreach(Type pluType in Aplugin.GetExportedTypes()){
                        if (pluType.IsPublic && pluType.IsClass && pluType.GetInterface(type.FullName) != null)
                        {
                            plugins.Add(Aplugin.CreateInstance(pluType.FullName));
                        }
                    }
                }

        }

        public void handleRequest(string url, Dictionary<string, string> data, StreamWriter outputStream)
        {
            foreach (iPlugin addin in plugins)
                {
                    if (addin.checkRequest(url) == true)
                    {   
                       addin.handleRequest(data, outputStream);
                       return;
                    }
                }
            writeResponse("<html><title>404 NOT FOUND</title><body><h1>404 Plugin Not Found</h1></body></html>", outputStream);
            return;
        }

        public List<String> getNames()
        {
            List<String> names = new List<String>();

//            names.Add("Hello");
            foreach (iPlugin addin in plugins)
            {
                names.Add(addin.getName());
            }

            return names;
        }

        private void writeResponse(string content, StreamWriter OutPutStream)
        {
            OutPutStream.WriteLine("HTTP/1.0 404 NOT FOUND");
            OutPutStream.WriteLine("Content-Type: text/html" );
            OutPutStream.WriteLine("Connection: close");
            OutPutStream.WriteLine("");

            OutPutStream.WriteLine(content);

        }
    }
}
