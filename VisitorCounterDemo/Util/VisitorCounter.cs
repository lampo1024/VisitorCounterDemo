using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.EntityFrameworkCore.Internal;

namespace VisitorCounter.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class VisitCounter
    {
        static VisitCounter()
        {
            var timer = new Timer(10000) { Enabled = true };
            timer.Start();
            timer.Elapsed += (sender, e) => VisitorList.RemoveWhere(x => x.LatestVisitAt < DateTime.Now.AddMinutes(-5));
        }

        public static void Visit(string visitorId)
        {
            var visitor = VisitorList.FirstOrDefault(x => x.Id == visitorId);
            if (visitor == null)
            {
                VisitorList.Add(new Visitor
                {
                    Id = visitorId,
                    FirstVisitAt = DateTime.Now,
                    LatestVisitAt = DateTime.Now
                });
            }
            else
            {
                visitor.LatestVisitAt = DateTime.Now;
            }
        }

        public static int VisitorNumber => VisitorList.Count;

        public static HashSet<Visitor> VisitorList { get; } = new HashSet<Visitor>();
    }

    /// <summary>
    /// Visitor
    /// </summary>
    public class Visitor
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// first visit time
        /// </summary>
        public DateTime FirstVisitAt { get; set; }
        /// <summary>
        /// latest visit time
        /// </summary>
        public DateTime LatestVisitAt { get; set; }
    }
}
