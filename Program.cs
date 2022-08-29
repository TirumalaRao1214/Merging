using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MergingApp
{
    internal class Program
    {
        static int storagefilename = 0;
        static void Main(string[] args)
        {
            
            foreach(var f in Directory.GetDirectories("C:/TirumalaRao/SourcePDFS"))
            {
               
                string[] files = new string[Directory.GetFiles(f).Length];
                int count = 0;
                foreach (var r in Directory.GetFiles(f))
                {
                    
                    files[count] = r;
                    count++;
                }
                MergePDF(files);
                
            }
            
        }

        private static void MergePDF(string[] fileArray)
        {
           
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = @"C:\TirumalaRao\DestinationPDFS\"+ storagefilename++ + ""+DateTime.Now.ToString("DDMMYYHHMMSS")+".pdf";

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int f = 0; f < fileArray.Length - 1; f++)
            {
                int pages = TotalPageCount(fileArray[f]);

                reader = new PdfReader(fileArray[f]);
                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();
        }

        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }
    }
}
