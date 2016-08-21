namespace DevMentor.Context.FileManager
{
    interface ITransactionsOperation
    {
        void Execute();

        void Commit();
    }
}
