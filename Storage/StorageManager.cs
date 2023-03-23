using NodeBlock.Engine.Storage.StorageAbstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Storage
{
    public static class StorageManager
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static IStorageAbstraction _storage;


        public static IStorageAbstraction GetStorage()
        {
            if(_storage == null)
            {
                switch (Environment.GetEnvironmentVariable("storage_engine"))
                {
                    case "local_storage":
                        logger.Info("Using LocalStorage for the storage engine");
                        if (!System.IO.Directory.Exists("./data"))
                        {
                            System.IO.Directory.CreateDirectory("./data");
                        }
                        _storage = new StorageAbstraction.LocalStorage.LocalStorage("./data/graphs.db");
                        break;

                    case "redis":
                        logger.Info("Using RedisStorage for the storage engine");
                        _storage = new StorageAbstraction.Redis.RedisStorage();
                        break;

                    // Set local storage as default
                    default:
                        logger.Info("No storage engine set in the env vars, using local_storage by default");
                        if (!System.IO.Directory.Exists("./data"))
                        {
                            System.IO.Directory.CreateDirectory("./data");
                        }
                        _storage = new StorageAbstraction.LocalStorage.LocalStorage("./data/graphs.db");
                        break;
                }
            }

            return _storage;
        }
    }
}
