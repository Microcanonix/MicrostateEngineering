namespace CommonDomain
{
    public static class AdditionalSymbols
    {
        /// <summary>
        /// No additonal charge
        /// </summary>
        public const string neutral = nameof(neutral);

        /// <summary>
        /// One electron less, positive charge HOMO
        /// </summary>
        public const string plus = nameof(plus);

        /// <summary>
        /// One electrin more , negative charge LUMO
        /// </summary>
        public const string minus = nameof(minus);
    }
}
