﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


namespace ZelderFramework.Maths
{
    /// <summary>
    /// Вектор целых чисел.
    /// </summary>
    public struct IntVector2
    {
        public Int32 X;
        public Int32 Y;
        public static IntVector2 Zero { get { return new IntVector2 { X = 0, Y = 0 }; }}
    }

    public struct Vector3Obj
    {
        public Vector3 Vector;
        public System.Object Obj;
        public static Vector3Obj Zero { get { return new Vector3Obj { Vector = Vector3.zero, Obj = null }; } }
    }


    /// <summary>
    /// Интерполятор.
    /// </summary>
    public static class Intepolator
    {
    
        static Vector3 Identity(Vector3 v) { return v; }
        static Vector3Obj IdentityObj(Vector3Obj v) { return v; }
        static Vector3 TransformDotPosition(Transform t) { return t.position; }

        public delegate Vector3 ToVector3<T>(T v);
        public delegate Vector3Obj ToVector3Obj<T>(T v);
        public delegate float Function(float a, float b, float c, float d);

        ///**
        // * Returns sequence generator from the first node, through each control point,
        // * and to the last node. N points are generated between each node (slices)
        // * using Catmull-Rom.
        // */
        //static IEnumerator NewCatmullRom(Transform[] nodes, int slices, bool loop)
        //{
        //    return NewCatmullRom(nodes, TransformDotPosition, slices, loop);
        //}


        /**
         * A Vector3[] variation of the Transform[] NewCatmullRom() function.
         * Same functionality but using Vector3s to define curve.
         */
        public static IEnumerable<Vector3> NewCatmullRom(Vector3[] points, int slices, bool loop)
        {
            return NewCatmullRom<Vector3>(points, Identity, slices, loop);
        }
        
        /**
         * Generic catmull-rom spline sequence generator used to implement the
         * Vector3[] and Transform[] variants. Normally you would not use this
         * function directly.
         */
        static IEnumerable<Vector3> NewCatmullRom<T>(IList nodes, ToVector3<T> toVector3, int slices, bool loop)
        {
            // need at least two nodes to spline between
            if (nodes.Count >= 2)
            {

                // yield the first point explicitly, if looping the first point
                // will be generated again in the step for loop when interpolating
                // from last point back to the first point
                yield return toVector3((T)nodes[0]);

                int last = nodes.Count - 1;
                for (int current = 0; loop || current < last; current++)
                {
                    // wrap around when looping
                    if (loop && current > last)
                    {
                        current = 0;
                    }
                    // handle edge cases for looping and non-looping scenarios
                    // when looping we wrap around, when not looping use start for previous
                    // and end for next when you at the ends of the nodes array
                    int previous = (current == 0) ? ((loop) ? last : current) : current - 1;
                    int start = current;
                    int end = (current == last) ? ((loop) ? 0 : current) : current + 1;
                    int next = (end == last) ? ((loop) ? 0 : end) : end + 1;

                    // adding one guarantees yielding at least the end point
                    int stepCount = slices + 1;
                    for (int step = 1; step <= stepCount; step++)
                    {
                        yield return CatmullRom(toVector3((T)nodes[previous]),
                                         toVector3((T)nodes[start]),
                                         toVector3((T)nodes[end]),
                                         toVector3((T)nodes[next]),
                                         step, stepCount);
                    }
                }
            }
        }

        /**
         * A Vector3 Catmull-Rom spline. Catmull-Rom splines are similar to bezier
         * splines but have the useful property that the generated curve will go
         * through each of the control points.
         *
         * NOTE: The NewCatmullRom() functions are an easier to use alternative to this
         * raw Catmull-Rom implementation.
         *
         * @param previous the point just before the start point or the start point
         *                 itself if no previous point is available
         * @param start generated when elapsedTime == 0
         * @param end generated when elapsedTime >= duration
         * @param next the point just after the end point or the end point itself if no
         *             next point is available
         */
        static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next,
                                    float elapsedTime, float duration)
        {
            // References used:
            // p.266 GemsV1
            //
            // tension is often set to 0.5 but you can use any reasonable value:
            // http://www.cs.cmu.edu/~462/projects/assn2/assn2/catmullRom.pdf
            //
            // bias and tension controls:
            // http://local.wasp.uwa.edu.au/~pbourke/miscellaneous/interpolation/

            float percentComplete = elapsedTime / duration;
            float percentCompleteSquared = percentComplete * percentComplete;
            float percentCompleteCubed = percentCompleteSquared * percentComplete;

            return previous * (-0.5f * percentCompleteCubed +
                                       percentCompleteSquared -
                                0.5f * percentComplete) +
                    start * (1.5f * percentCompleteCubed +
                               -2.5f * percentCompleteSquared + 1.0f) +
                    end * (-1.5f * percentCompleteCubed +
                                2.0f * percentCompleteSquared +
                                0.5f * percentComplete) +
                    next * (0.5f * percentCompleteCubed -
                                0.5f * percentCompleteSquared);
        }





        /*
        public static IEnumerable<Vector3Obj> NewCatmullRom(Vector3Obj[] points, int slices, bool loop)
        {
            return NewCatmullRomObj<Vector3Obj>(points, IdentityObj, slices, loop);
        }
        static IEnumerable<Vector3Obj> NewCatmullRomObj<T>(IList nodes, ToVector3Obj<T> toVector3, int slices, bool loop)
        {
            // need at least two nodes to spline between
            if (nodes.Count >= 2)
            {

                // yield the first point explicitly, if looping the first point
                // will be generated again in the step for loop when interpolating
                // from last point back to the first point
                yield return toVector3((T)nodes[0]);

                int last = nodes.Count - 1;
                for (int current = 0; loop || current < last; current++)
                {
                    // wrap around when looping
                    if (loop && current > last)
                    {
                        current = 0;
                    }
                    // handle edge cases for looping and non-looping scenarios
                    // when looping we wrap around, when not looping use start for previous
                    // and end for next when you at the ends of the nodes array
                    int previous = (current == 0) ? ((loop) ? last : current) : current - 1;
                    int start = current;
                    int end = (current == last) ? ((loop) ? 0 : current) : current + 1;
                    int next = (end == last) ? ((loop) ? 0 : end) : end + 1;

                    // adding one guarantees yielding at least the end point
                    int stepCount = slices + 1;
                    for (int step = 1; step <= stepCount; step++)
                    {
                        yield return CatmullRomObj(toVector3((T)nodes[previous]),
                                         toVector3((T)nodes[start]),
                                         toVector3((T)nodes[end]),
                                         toVector3((T)nodes[next]),
                                         step, stepCount);
                    }
                }
            }
        }

        static Vector3Obj CatmullRomObj(Vector3Obj previous, Vector3Obj start, Vector3Obj end, Vector3Obj next, float elapsedTime, float duration)
        {
            // References used:
            // p.266 GemsV1
            //
            // tension is often set to 0.5 but you can use any reasonable value:
            // http://www.cs.cmu.edu/~462/projects/assn2/assn2/catmullRom.pdf
            //
            // bias and tension controls:
            // http://local.wasp.uwa.edu.au/~pbourke/miscellaneous/interpolation/

            float percentComplete = elapsedTime / duration;
            float percentCompleteSquared = percentComplete * percentComplete;
            float percentCompleteCubed = percentCompleteSquared * percentComplete;

            return previous * (-0.5f * percentCompleteCubed + percentCompleteSquared - 0.5f * percentComplete) +
                    start * (1.5f * percentCompleteCubed + -2.5f * percentCompleteSquared + 1.0f) +
                    end * (-1.5f * percentCompleteCubed + 2.0f * percentCompleteSquared + 0.5f * percentComplete) +
                    next * (0.5f * percentCompleteCubed - 0.5f * percentCompleteSquared);
        }
        */

    }

}