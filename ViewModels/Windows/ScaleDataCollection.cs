namespace MaterialDemo.ViewModels.Windows
{

    public class ScaleDataCollection
    {

        private readonly Thread workThread;
        private volatile bool workDone = true;

        private readonly Dictionary<string, int> scaleValuePairs = [];

        public ScaleDataCollection()
        {
            workThread = new Thread(CollectTask);
        }

        public void CollectTask()
        {
            while (workDone)
            {

            }
        }
    }
}
