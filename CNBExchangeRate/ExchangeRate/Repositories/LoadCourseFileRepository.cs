using ExchangeRate.Entities;
using ExchangeRate.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Repositories
{

  /*
   * Class for read file from CNB url or Temp dir
   */
  class LoadCourseFileRepository
  {
    private string prefix = "LoadCourseFile"; //prefix file name to be save in temp
    private string sufix = ".txt"; //sufix file name to be save in temp
    private string filePath = null; // full path file to be save in temp
    private string dateFileFormat = "dd.MM.yyyy"; // date format for path file
    private string filePrevious = null; // full path file to be save in temp yestrday
    private string cnbUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date="; //file link for CNB foreign exchange market

    /**
     * main function for load file from CNB URL or file in Temp dir
     */
    public StreamReader ReadCourse()
    {
      //if exist file in Temp load CNB exchange to Temp for today
      if (existsFileInTemp( getFilePath() ) == true)
      {
        return LoadFileByTemp();//StreamReader file from TEMP 
      }
      else
      {
        DeleteFilePrevious();//previose file delete
        StreamReader reader = ReadCourseByUrl();//reader from CNB URL
        SaveFileToTemp(reader);//save StreamReader from CNB url to file in TEMP
        return LoadFileByTemp();//StreamReader file from TEMP 
      }
    }

    /*
     * stream reader from CNB URL
     */
    public StreamReader ReadCourseByUrl()
    {
      string dt = Today();
      string url = cnbUrl+ dt;
      try
      {
        WebClient client = new WebClient();
        Stream stream = client.OpenRead(url);
        StreamReader reader = new StreamReader(stream);
        return reader;
      }
      catch (Exception ext)
      {
        throw new FileException(Status.WEB_CLIENT_ERR, string.Format(@"failed to connect {0}.", url), ext);
      }
    }

    /*
     *return string Today format is dd.MM.yyyy 
     */
    private string Today()
    {
      DateTime date = DateTime.Now;
      string ret = date.ToString(dateFileFormat);
      return ret;
    }

    /*
     * return string file full path in Temp dir
     */
    private string getFilePath()
    {
      if (filePath == null)
      {
        string dt = Today();
        string fileName = prefix + dt + sufix;
        filePath = Path.Combine(Path.GetTempPath(), fileName);
      }
      return filePath;
    }

    //return string file previose full path in Temp dir
    private string getFilePrevious()
    {
      if (filePrevious == null)
      {
        filePrevious = prefix +"*"+ sufix;
      }
      return filePrevious;
    }

    /**
     * exists file in TEMP dir
     */
    private bool existsFileInTemp( string filePath )
    {
      return File.Exists(filePath);
    }

    /**
     * save StreamReader from CNB url to file in TEMP
     */
    private void SaveFileToTemp(StreamReader reader)
    {
      string line;
      try
      {
        // Write each directory name to a file.
        using (StreamWriter sw = new StreamWriter(getFilePath()))
        {
          while ((line = reader.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
      }
      catch (Exception ext)
      {
        throw new FileException(Status.SAVE_FILE_ERR, string.Format(@"File {0} failed to save.", getFilePath()), ext);
      }
    }

    /**
     *return StreamReader file from TEMP 
     */
    private StreamReader LoadFileByTemp()
    {
      try
      {   // Open the text file using a stream reader.
        StreamReader sr = new StreamReader(getFilePath());
        return sr;
      }
      catch (Exception ext)
      {
        throw new FileException(Status.LOAD_FILE_ERR, string.Format(@"File {0} failed to load.", getFilePath()), ext);
      }
    }

    //delete previious file
    private void DeleteFilePrevious()
    {
      string filesDelete = "*"+getFilePrevious();
      string folderPath = Path.GetTempPath();
      try
      { 
        string[] fileList = System.IO.Directory.GetFiles(folderPath, filesDelete);
        //if exist file previos delete this file
        foreach (string file in fileList)
        {
          File.Delete(file);
        }
      }
      catch (Exception ext)
      {
        throw new FileException(Status.SAVE_FILE_ERR, string.Format(@"Files in {0} failed to delete.", folderPath), ext);
      }
    }
  }
}
