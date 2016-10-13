using System;

namespace Nop.Core.Domain.Customers
{
    /// <summary>
    /// Represents the period of delay
    /// </summary>
    public enum RewardPointsAccruingDelayPeriod
    {
        /// <summary>
        /// Hours
        /// </summary>
        Hours = 0,
        /// <summary>
        /// Days
        /// </summary>
        Days = 1
    }

    /// <summary>
    /// RewardPointsAccruingDelayPeriod Extensions
    /// </summary>
    public static class RewardPointsAccruingDelayPeriodExtensions
    {
        /// <summary>
        /// Returns a delay period before accruing points in hours
        /// </summary>
        /// <param name="period">Reward points accruing delay period</param>
        /// <param name="value">Value of delay</param>
        /// <returns>Value of delay in hours</returns>
        public static int ToHours(this RewardPointsAccruingDelayPeriod period, int value)
        {
            switch (period)
            {
                case RewardPointsAccruingDelayPeriod.Hours:
                    return value;
                case RewardPointsAccruingDelayPeriod.Days:
                    return value * 24;
                default:
                    throw new ArgumentOutOfRangeException("RewardPointsAccruingDelayPeriod");
            }
        }
    }
}