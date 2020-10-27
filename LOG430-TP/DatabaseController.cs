using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
  public  class DatabaseController
    {


        /// <summary>
        /// Save un application Message dans la DB
        /// </summary>
        /// <param name="applicationMessage"></param>
        public void add(ApplicationMessage applicationMessage)
        {
            //TODO

        }

        /// <summary>
        /// Retourne une liste d'Application Message qui match le topic et la date de début et de fin
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public ApplicationMessage GetTopic(string dateDebut, string dateFin, string topic)
        {
            //TODO
        }

        /// <summary>
        /// Retourne une liste de toutes les Application Message après la date de début
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <returns></returns>
        public ApplicationMessage GetTopic(string dateDebut)
        {
            //TODO
        }

    }
}
