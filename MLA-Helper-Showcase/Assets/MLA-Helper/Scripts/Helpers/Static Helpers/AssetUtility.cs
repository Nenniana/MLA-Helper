using System.IO;
using UnityEditor;

// Static Asset Utility which may create unique folders or asset names.
namespace MLAHelper {
    public class AssetUtility
    {
        public static string CreateOrGetFolder(string path, string folderName)
        {
            string folderPath = Path.Combine(path, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }

            return folderPath;
        }

        public static string CreateUniqueFolder(string path, string folderName) 
        {
            string uniqueFolderPath = Path.Combine(path, folderName);

            if (Directory.Exists(uniqueFolderPath)) {
                uniqueFolderPath = GenerateUniqueFolderName(path, folderName);
            }

            Directory.CreateDirectory(uniqueFolderPath);
            AssetDatabase.Refresh();

            return uniqueFolderPath;
        }

        public static string GenerateUniqueFolderName(string path, string folderName)
        {
            string uniqueFolderName = folderName;
            int counter = 1;

            while (Directory.Exists(Path.Combine(path, uniqueFolderName)))
            {
                uniqueFolderName = folderName + "_" + counter;
                counter++;
            }

            return Path.Combine(path, uniqueFolderName);
        }

        public static string GenerateUniqueAssetName(string folderPath, string assetName, string fileExtension)
        {
            string uniqueAssetName = assetName;
            int counter = 1;

            while (File.Exists(Path.Combine(folderPath, uniqueAssetName + fileExtension)))
            {
                uniqueAssetName = assetName + "_" + counter;
                counter++;
            }

            return uniqueAssetName;
        }

        public static string GenerateUniqueAssetPath(string folderPath, string assetName, string fileExtension) {
            string uniqueAssetName = GenerateUniqueAssetName(folderPath, assetName, fileExtension);

            return Path.Combine(folderPath, uniqueAssetName + fileExtension);
        }

        public static string CreateOrGetUniqueAsset(string folderPath, string assetName, string fileExtension)
        {
            string uniqueAssetName = GenerateUniqueAssetName(folderPath, assetName, fileExtension);

            return uniqueAssetName;
        }
    }
}