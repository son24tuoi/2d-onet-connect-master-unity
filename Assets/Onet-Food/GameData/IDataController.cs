public interface IDataController
{
    public void LoadData();
    public void LoadData(string jsonData);
    public string GetData();
    public void SaveData();
    public void ClearData();
    public void DeleteData();
}