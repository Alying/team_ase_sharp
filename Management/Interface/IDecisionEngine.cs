﻿// <copyright file="IDecisionEngine.cs" company="ASE#">
//     Copyright (c) ASE#. All rights reserved.
// </copyright>

namespace Management.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Management.DomainModels;

    /// <summary>
    /// Interface for recommendation's decision engine 
    /// </summary>
    public interface IDecisionEngine
    {
        /// <summary>
        /// Calculate the desired location using weighted scores from COVID-19, weather, and air quality
        /// </summary>
        /// <returns>The weighted score.</returns>
        Task<IEnumerable<Recommendation>> CalculateDesiredLocationAsync();

        /// <summary>
        /// Gets the specific location's information 
        /// </summary>
        /// <param name="location">The country and state the user inquired.</param>
        /// <param name="userId">The user's unique id.</param>
        /// <returns>The state's information.</returns>
        Task<Recommendation> GetSpecificLocationInfoAsync(
                             Location location,
                             UserId userId);
    }
}
