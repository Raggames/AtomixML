using System.Threading.Tasks;
using UnityEngine;


namespace Atom.MachineLearning.Core
{
    /// <summary>
    /// Abstraction d'un trainer
    /// Un mod�le/algorithme va toujours de paire avec son ITrainer, qui impl�mente toutes les fonctions d'apprentissage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMLTrainer<T> where T : IMLTrainingDatas
    {
        public Task<ITrainingResult> Fit(T trainingDatas);
    }
}
