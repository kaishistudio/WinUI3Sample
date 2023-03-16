﻿using System.Text;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace WinUI3Sample.Services;
public class KSFileService
{
    /// <summary>
    /// 各种路径
    /// </summary>
    public StorageFolder InstalledLocation = Package.Current.InstalledLocation;
    public StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
    public string DesktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    public string MyDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public string StartMenuFolder = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
    public string MyPicturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
    public string MyMusicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
    public string MyVideosFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
    /// <summary>
    /// 选择文件夹
    /// </summary>
    /// <returns></returns>
    public async Task<StorageFolder> ChooseFolder()
    {
        var folderPicker = new FolderPicker();
        folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        folderPicker.FileTypeFilter.Add("*");
        StorageFolder folder = await folderPicker.PickSingleFolderAsync();
        return folder;
    }
    /// <summary>
    /// 选择文件
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="filetypes"></param>
    /// <returns></returns>
    public async Task<StorageFile> ChooseFile(PickerLocationId pid, string[] filetypes)
    {
        FileOpenPicker openPicker = new FileOpenPicker();
        openPicker.ViewMode = PickerViewMode.List;
        openPicker.SuggestedStartLocation = pid;
        foreach (string filetype in filetypes)
        {
            openPicker.FileTypeFilter.Add(filetype);
        }
        var file = await openPicker.PickSingleFileAsync();
        return file;
    }
    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="info"></param>
    /// <param name="types"></param>
    /// <param name="filename"></param>
    /// <param name="txt"></param>
    async public void SaveFile(string info, List<string> types, string filename, string txt)
    {
        var saveFile = new FileSavePicker();
        saveFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        saveFile.FileTypeChoices.Add(info, types);
        saveFile.SuggestedFileName = filename;
        StorageFile sFile = await saveFile.PickSaveFileAsync();
        if (sFile != null)
        {
            using (StorageStreamTransaction transaction = await sFile.OpenTransactedWriteAsync())
            {
                using (DataWriter dataWriter = new DataWriter(transaction.Stream))
                {
                    dataWriter.WriteString(txt);
                    transaction.Stream.Size = await dataWriter.StoreAsync();
                    await transaction.CommitAsync();
                }
            }
        }
    }
    /// <summary>
    /// 往LocalFolder中写文件
    /// </summary>
    /// <param name="dirname"></param>
    /// <param name="name"></param>
    /// <param name="txt"></param>
    public async void WriteFileInLocalFolder(string dirname, string name, string txt)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname == "")
            {
                storageFolder = ApplicationData.Current.LocalFolder;
            }
            else
            {
                storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(dirname, CreationCollisionOption.OpenIfExists);
            }
            StorageFile file = await storageFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, txt);
        }
        catch { }
    }
    /// <summary>
    /// 往LocalFolder中读文件
    /// </summary>
    /// <param name="dirname"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<string> ReadFileInLocalFolder(string dirname, string name)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname == "")
            {
                storageFolder = ApplicationData.Current.LocalFolder;
            }
            else
            {
                storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(dirname, CreationCollisionOption.OpenIfExists);
            }
            StorageFile file = await storageFolder.GetFileAsync(name);
            return await FileIO.ReadTextAsync(file);
        }
        catch
        {
            return string.Empty;
        }
    }
    /// <summary>
    /// 往LocalFolder中删文件
    /// </summary>
    /// <param name="dirname"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<bool> DelFileInLocalFolder(string dirname, string name)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname == "")
            {
                storageFolder = ApplicationData.Current.LocalFolder;
            }
            else
            {
                storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(dirname);
            }
            StorageFile file = await storageFolder.GetFileAsync(name);
            await file.DeleteAsync();
            return true;
        }
        catch { return false; }
    }
    /// <summary>
    /// 往LocalFolder中获得文件列表
    /// </summary>
    /// <param name="dirname"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<StorageFile>> GetFilesInLocalFolder(string dirname)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname == "")
            {
                storageFolder = ApplicationData.Current.LocalFolder;
            }
            else
            {
                storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(dirname);
            }
            return await storageFolder.GetFilesAsync();
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 判断LocalFolder中文件存在
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="dirname"></param>
    /// <returns></returns>
    public async Task<bool> DoesFileExistAsyncInLocalFolder(string fileName, string dirname)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname != "")
                storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(dirname);
            else
                storageFolder = ApplicationData.Current.LocalFolder;
            await storageFolder.GetFileAsync(fileName);
            return true;
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// 读取StorageFile的txt
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<string> ReadFileFromStorageFile(StorageFile file)
    {
        string str = "";
        try
        {
            str = await Windows.Storage.FileIO.ReadTextAsync(file);
        }
        catch (ArgumentOutOfRangeException)
        {
            //using(var stream =new StreamReader((await file.OpenReadAsync()).GetInputStreamAt(0).AsStreamForRead()))
            //{
            //    string text = stream.ReadToEnd();
            //    return text;
            //}
            IBuffer buffer = await FileIO.ReadBufferAsync(file);
            DataReader reader = DataReader.FromBuffer(buffer);
            byte[] fileContent = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(fileContent);
            string text = "";

            // Encoding.ASCII.GetString(fileContent, 0, fileContent.Length);

            //text= Encoding.GetEncoding(0).GetString(fileContent, 0, fileContent.Length);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding gbk = Encoding.GetEncoding("GBK");

            text = gbk.GetString(fileContent);
            //string text = AutoEncoding(new byte[4] { fileContent[0], fileContent[1], fileContent[2], fileContent[3] }).GetString(fileContent);

            return text;
        }
        return str;
    }
   
}