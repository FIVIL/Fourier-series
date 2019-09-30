# Fourier Series Visualizer
   
![Fourier Series Visualizer](Furier1.JPG?raw=true "Fourier Series Visualizer")

## Description

In mathematics, a Fourier series is a periodic function composed of harmonically related sinusoids, combined by a weighted summation. With appropriate weights, one cycle (or period) of the summation can be made to approximate an arbitrary function in that interval (or the entire function if it too is periodic). As such, the summation is a synthesis of another function. The discrete-time Fourier transform is an example of Fourier series. The process of deriving the weights that describe a given function is a form of Fourier analysis. For functions on unbounded intervals, the analysis and synthesis analogies are Fourier transform and inverse transform. ([wikipedia](https://en.wikipedia.org/wiki/Fourier_series))

## This Project

This project is a simple visualizer for a Fourier series which can calculate and depict the Fourier series of a given function (Either simple or piecewise) with the required precision. Besides, it can perform a Half-range expansion of a function before calculating the Fourier series.

## How It Works

This application uses [Simpson's rule](https://en.wikipedia.org/wiki/Simpson%27s_rule) for calculating harmonics and the rest is magic!!!

## Clone

You can simply clone this repo and run the code, using Visual Studio 2017+, You need to add a reference to WPF [syncfusion](https://www.syncfusion.com) chart controls in order to run this project successfully.