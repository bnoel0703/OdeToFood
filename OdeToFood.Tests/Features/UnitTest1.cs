using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OdeToFood.Models;

namespace OdeToFood.Tests.Features
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Compute_Result_For_One_Review()
        {
            var data = BuildRestaurantAndReviews(ratings: 4);

            var rater = new RestaurantRater(data);
            var result = rater.ComputeRating(10);

            Assert.AreEqual(4, result.Rating);
        }

        [TestMethod]
        public void Compute_Result_For_Two_Reviews()
        {
            var data = BuildRestaurantAndReviews(4, 8);


            var rater = new RestaurantRater(data);
            var result = rater.ComputeRating(10);

            Assert.AreEqual(6, result.Rating);
        }

        [TestMethod]
        public void Weighted_Average_Of_Two_Reviews()
        {
            var data = BuildRestaurantAndReviews(3, 9);

            var rater = new RestaurantRater(data);
            var result = rater.ComputeWeightedRate(10);

            Assert.AreEqual(6, result.Rating);
        }

        private Restaurant BuildRestaurantAndReviews(params int[] ratings)
        {
            var restaurant = new Restaurant();

            restaurant.Reviews = ratings.Select(r => new RestaurantReview {Rating = r}).ToList();

            return restaurant;
        }

    }

    public class RestaurantRater
    {
        private Restaurant _restaurant;
        public RestaurantRater(Restaurant restaurant)
        {
            this._restaurant = restaurant;
        }

        public RatingResult ComputeRating(int numberOfReviews)
        {
            var result = new RatingResult();
            result.Rating = (int)_restaurant.Reviews.Average(r => r.Rating);
            return result;
        }

        public RatingResult ComputeWeightedRate(int numberOfReviews)
        {
            var reviews = _restaurant.Reviews.ToArray();
            var result = new RatingResult();
            var counter = 0;
            var total = 0;

            for (int i = 0; i < reviews.Count(); i++)
            {
                if (i < reviews.Count() / 2)
                {
                    counter += 2;
                    total += reviews[i].Rating * 2;
                }
                else
                {
                    counter += 1;
                    total += reviews[i].Rating;
                }
            }

            result.Rating = total / counter;
            return result;
        }
    }

    public class RatingResult
    {
        public int Rating;
    }
}
