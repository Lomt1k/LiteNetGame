namespace Networking.Server
{
    //TODO вынести в JSON чтобы менять настройки в папке сервера, клиент должен подтягивать эти настройки при подключении
    public struct ServerConfig
    {
        public float syncUnitState_rate; //частота синхронизации передвижения и анимации юнита в милисекундах
        public float minFloatChangeSync; //минимальное изменение float, которое будет синхронизироваться по сети

        public static ServerConfig defaultConfig => new ServerConfig
        {
            syncUnitState_rate = 0.05f,
            minFloatChangeSync = 0.01f
        };
        
        
    }
}

