using System.Threading.Tasks;
using UnityEngine;

namespace Academy
{
    public class Environment
    {
    
        private static int vertical, horizontal;
        private NomGrid nomGrid;
        private Agent agent;
        private float epsilon;
        private bool enable;
        private float NOMS_PER_ENV;
    
    
        public void EnvSetupBase(float NPE)
        {
            horizontal = 5; //(int)Camera.main.orthographicSize;
            vertical = 5; //(int)Camera.main.orthographicSize;
            NOMS_PER_ENV = NPE;
        }

        public void EnvSet(float ep, string strategy)
        {
            epsilon = ep;

            nomGrid = new NomGrid(horizontal, vertical);
            nomGrid.initGrid(NOMS_PER_ENV);

            agent = new Agent(strategy);
            agent.initPos(horizontal, vertical);
        }

        public void EnvReset()
        {
            nomGrid = null;
        
            agent = null;
        }

        public int RunEnv()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (nomGrid.getNomCount() <= 0)
                {
                    break;
                }
                
                Task action = Task.Factory.StartNew(() => 
                    agent.action(nomGrid.getState(agent.GetRow(), agent.GetCol()), nomGrid, epsilon));
                
                action.Wait();
            }
            
            return agent.GetFitness();
        }

        public bool[,] getNomGrid()
        {
            return nomGrid.getGrid();
        }

    }
}
