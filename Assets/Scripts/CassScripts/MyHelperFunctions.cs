using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;
// using UnityEngine;
// using System.Text.Json;

namespace MyHelperFunctions
{
	public class MY{

		static public long safeDivide(long a, long b){
			long INF = long.MaxValue;
			long NEG_INF = long.MinValue;

			if(a == 0 && b == 0) return 0;
			if(a == 0 && (b == INF || b == NEG_INF)) return 0;
			if((a == INF || a == NEG_INF) && b == 0) return a;
			if((a == INF || a == NEG_INF) && (b == INF || b == NEG_INF)){
				return (a == b ? 1 : -1);
			}
			if(b == 0){
				return (a > 0 ? INF : NEG_INF);
			}
			if(b == INF || b == -INF) return 0;
			
			return (a / b);
		}static public double safeDivide(double a, double b){
			double INF = double.PositiveInfinity;
			double NEG_INF = double.NegativeInfinity;

			if(a == 0 && b == 0) return 0;
			if(a == 0 && (b == INF || b == NEG_INF)) return 0;
			if((a == INF || a == NEG_INF) && b == 0) return a;
			if((a == INF || a == NEG_INF) && (b == INF || b == NEG_INF)){
				return (a == b ? 1 : -1);
			}
			if(b == 0){
				return (a > 0 ? INF : NEG_INF);
			}
			if(b == INF || b == -INF) return 0;
			
			return (a / b);
		}static public float safeDivide(float a, float b){
			float INF = float.PositiveInfinity;
			float NEG_INF = float.NegativeInfinity;

			if(a == 0 && b == 0) return 0;
			if(a == 0 && (b == INF || b == NEG_INF)) return 0;
			if((a == INF || a == NEG_INF) && b == 0) return a;
			if((a == INF || a == NEG_INF) && (b == INF || b == NEG_INF)){
				return (a == b ? 1 : -1);
			}
			if(b == 0){
				return (a > 0 ? INF : NEG_INF);
			}
			if(b == INF || b == -INF) return 0;
			
			return (a / b);
		}
		static public bool isInRange<T>(T num, T min, T max, bool inclusive = true) where T : IComparable
		{
			if(inclusive){
				return (num.CompareTo(min) >= 0 && num.CompareTo(max) <= 0);
			}else{
				return (num.CompareTo(min) > 0 && num.CompareTo(max) < 0);
			}
		}

		static public int mod(int a, int b){
			return ((a % b) + b) % b;
		}
		static public long mod(long a, long b){
			return ((a % b) + b) % b;
		}static public decimal mod(decimal a, decimal b){
			return ((a % b) + b) % b;
		}static public float mod(float a, float b){
			return ((a % b) + b) % b;
		}static public double mod(double a, double b){
			return ((a % b) + b) % b;
		}

		static public long sumOfArray(long[] arr, Func<long,long> additionFunc = null){
			if(additionFunc == null){
				additionFunc = (x) => {
					return x;
				};
			}
			long sum = 0;
			foreach(long item in arr){
				sum += additionFunc(item);
			}
			return sum;
		}static public decimal sumOfArray(decimal[] arr, Func<decimal,decimal> additionFunc = null){
			if(additionFunc == null){
				additionFunc = (x) => {
					return x;
				};
			}
			decimal sum = 0;
			foreach(decimal item in arr){
				sum += additionFunc(item);
			}
			return sum;
		}

		static public List<Vector2> getClosestPathInCircle(List<Vector2> arr, int _from, int _to, bool clockwiseBias = true){
			return MY.getClosestPathInCircle(arr.ToArray(), _from, _to, clockwiseBias);
		}
		static public List<Vector2> getClosestPathInCircle(Vector2[] arr, int _from, int _to, bool clockwiseBias = true){
			if(!MY.isInRange<int>(_from, 0, arr.Length-1) || !MY.isInRange<int>(_to, 0, arr.Length-1)){
				return new List<Vector2>();
			}

			int iL, iR; iL = iR = _from;

			List<Vector2> arrL = new List<Vector2>();
			List<Vector2> arrR = new List<Vector2>();
			List<Vector2> arrX;

			while(arrL.Count < arr.Length){
				int _i = (int) MY.mod(iL, arr.Length);

				arrL.Add(arr[_i]);
				if(Math.Abs(_i) == _to) break;
				iL --;
			}
			while(arrR.Count < arr.Length){
				int _i = (int) MY.mod(iR, arr.Length);

				arrR.Add(arr[_i]);
				if(Math.Abs(_i) == _to) break;
				iR --;
			}

			if(Math.Abs(arrL.Count) < Math.Abs(arrR.Count)){
				arrX = arrL;
			}else if(Math.Abs(arrL.Count) > Math.Abs(arrR.Count)){
				arrX = arrR;
			}else{
				if(clockwiseBias) arrX = arrR;
				else arrX = arrL;
			}

			return arrX;
			
		}

		public static long Lerp(long a, long b, long t) {
			return a + t * ( b - a );
		}public static long LerpT(long a, long b, long L) {
			if (b==a) return long.MaxValue;
			return (L-a)/(b-a);
		}
		public static double Lerp(double a, double b, double t) {
			return a + t * ( b - a );
		}public static double LerpT(double a, double b, double L) {
			if (b==a) return double.PositiveInfinity;
			return (L-a)/(b-a);
		}
		public static float Lerp(float a, float b, float t) {
			return a + t * ( b - a );
		}public static float LerpT(float a, float b, float L) {
			if (b==a) return float.PositiveInfinity;
			return (L-a)/(b-a);
		}


	}
}