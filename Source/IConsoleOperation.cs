namespace Consolix
{
    /// <summary>
    /// Use on business logic class to mark it as handler for certain operation.
    /// </summary>
    public interface IConsoleOperation
    {
        /// <summary>
        /// OperationName of the operation.
        /// In case of multiple operations it will be compared to CommandLine arguments for its name as indication for being selected.
        /// </summary>
        string OperationName { get; }

        /// <summary>
        /// Help text to show in case of asking help from command line or having insufficient data for it (lacking operands).
        /// </summary>
        string HelpText { get; }

        /// <summary>
        /// Actual method which does the work.
        /// </summary>
        /// <returns>Status code (0 = OK, other = problem)</returns>
        Task<int> DoWork();

        /// <summary>
        /// Flag, indicating whether Operation has sufficient amount of option values to perform its work.
        /// </summary>
        /// <value>
        /// <c>true</c> if all is OK; otherwise, <c>false</c>.
        /// </value>
        bool IsReady { get; }
    }
}
