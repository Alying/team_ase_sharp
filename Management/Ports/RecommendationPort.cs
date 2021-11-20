﻿// <copyright file="RecommendationPort.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Management.DomainModels;
using Management.Interface;
using Management.Mapping;

namespace Management.Ports
{
    /// <summary>
    /// Recommendation ports for async task.
    /// </summary>
    public class RecommendationPort
    {
        private readonly IDecisionEngine _decisionEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendationPort"/> class.
        /// </summary>
        /// <param name="decisionEngine">decision engine for recommendation port.</param>
        public RecommendationPort(IDecisionEngine decisionEngine)
        {
            _decisionEngine = decisionEngine ?? throw new ArgumentNullException(nameof(decisionEngine));
        }

        /// <summary>
        /// Get cities with their covid, weather, and air quality scores.
        /// </summary>
        /// <param name="country">country of interest eg. US.</param>
        /// <param name="state">state of interest eg. NY.</param>
        /// <param name="cancellationToken">used to signal that the asynchronous task should cancel itself.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation, with a status code.</returns>
        public async Task<Dictionary<string, double>> GetRecommendationsCitiesWithScoreAsync(
            string country,
            string state,
            CancellationToken cancellationToken)
        {
            var (validatedCountry, validatedState) = CountryStateValidator.ValidateCountryState(country, state);

            var result = await _decisionEngine.CalculateDesiredLocationAsync(validatedState, validatedCountry, cancellationToken);

            return result.ToDictionary(kvp => kvp.Key.Value, kvp => kvp.Value);
        }

        /// <summary>
        /// Gets the specific location's information. 
        /// </summary>
        /// <param name="location">The country and state the user inquired.</param>
        /// <param name="userId">The user's unique id.</param>
        /// <returns>The state's information.</returns>
        public async Task<Recommendation> GetLocationInfoAsync(Location location, UserId userId)

            => await _decisionEngine.GetSpecificLocationInfoAsync(location, userId);
    }
}
