using System;
using System.Collections.ObjectModel;
using System.Linq;
using ContradiccionesDirectorioApi.Dao;
using ContradiccionesDirectorioApi.Model;

namespace ContradiccionesDirectorioApi.Singletons
{
    public class FuncionariosSingleton
    {
        private static ObservableCollection<Funcionarios> funcionarios;

        private FuncionariosSingleton()
        {
        }

        public static ObservableCollection<Funcionarios> FuncionariosCollection
        {
            get
            {
                if (funcionarios == null)
                    funcionarios = new FuncionariosModel().GetFuncionarios(1);

                return funcionarios;
            }
        }



        public static void AddFuncionario(Funcionarios funcionario)
        {
            FuncionariosSingleton.funcionarios.Add(funcionario);
        }

        public static void RemoveFuncionario(Funcionarios funcionario)
        {
            funcionarios.Remove(funcionario);
        }
    }
}
