﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace SmartSocketWindow.ViewModels
{
    class DaysChartModel
    {
        public DaysChartModel()
        {
            this.MyModel = new PlotModel { Title = "Example 1" };
            this.MyModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
        }

        public PlotModel MyModel { get; private set; }
    }
}
