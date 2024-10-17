using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Reflection.PortableExecutable;
using System.Diagnostics;
using System;
using System.Reflection.Metadata.Ecma335;

namespace BoundingMeshesClass
{
    public class Face
    {
        private Vector3 _position; 
        public Vector3 position { get{ return _position; } set{ _position = value; calcPlane(); } }
        private Vector3 _normal;
        public Vector3 normal { get{ return _normal; } set{ _normal = value; calcPlane(); } }

        public Plane plane;
        public Vector2[] vertices;

        int sides;
        public float scale;
        public float width;
        public float height;

        public Face()
        {
            this.plane = new Plane(Vector3.Zero, Vector3.Up);
            
            this.position = Vector3.Zero;
            this.normal = Vector3.Up;

            this.sides = 4;

            this.scale = 1f;
            this.height = 1f;
            this.width = 1f;

            this.vertices = new Vector2[sides];

            for(int i = 0; i < sides; i++)
            {
                float angle = i * MathHelper.TwoPi/sides;
                this.vertices[i] = new Vector2(MathF.Sin(angle), MathF.Cos(angle)) * scale;
            }
        }

        public Face(Vector3 position, Vector3 normal, int sides, float scale)
        {
            this.plane = new Plane(position, normal);

            this.position = position;
            this.normal = normal;

            this.sides = sides;

            this.scale = scale;
            this.height = 1f;
            this.width = 1f;

            this.vertices = new Vector2[sides];

            for(int i = 0; i < sides; i++)
            {
                float angle = i * MathHelper.TwoPi/sides;
                this.vertices[i] = new Vector2(MathF.Sin(angle), MathF.Cos(angle)) * scale;
            }
        }

        public Face(Vector3 position, Vector3 normal, float width, float height)
        {
            this.plane = new Plane(position, normal);

            this.position = position;
            this.normal = normal;

            this.sides = 4;

            this.scale = 1f;
            this.width = width;
            this.height = height;

            this.vertices = new Vector2[4];

            vertices[3] = new Vector2(-0.5f * width, -0.5f * height);      
            vertices[1] = new Vector2(-0.5f * width, 0.5f * height);      
            vertices[0] = new Vector2(0.5f * width, 0.5f * height);
            vertices[2] = new Vector2(0.5f * width, -0.5f * height);      
        }

        private void calcPlane()
        {
            this.plane = new Plane(this.position, this.normal);
        }

        public Face clone()
        {
            Face temp = new Face();

            temp.normal = this.normal;
            temp.position = this.position;

            temp.vertices = this.vertices;
            temp.plane = this.plane;

            temp.sides = this.sides;

            temp.scale = this.scale;
            temp.width = this.width;
            temp.height = this.height;

            return temp;
        }
    }

    public class BoundingMesh 
    {
        private Face[] orignialFaces;
        public Face[] faces;

        private BoundingBox orignalBox;
        public BoundingBox box {get; private set;}

        private Vector3 _position;
        public Vector3 position { get { return _position; } set { _position = value; reCalcTranslation(); } }

        private Vector3 _rotation;
        public Vector3 rotation { get { return _rotation; } set { _rotation = value; reCalcRotation(); } }

        public Vector3 localUp = Vector3.Up;
        public Vector3 localForward = Vector3.Forward;

        public BoundingMesh(Face[] faces, Vector3 position, Vector3 rotation)
        {
            this._position = position;
            this._rotation = rotation;

            Debug.Assert(faces.Length > 0);

            this.orignialFaces = new Face[faces.Length];
            this.faces = faces;

            for (int i = 0; i < faces.Length; i++)
            {
                orignialFaces[i] = faces[i].clone();
            }

            reCalcRotation();
            reCalcTranslation();           

            this.faces = faces;
            Vector3 max = faces[0].position;
            Vector3 min = faces[0].position;

            foreach(Face face in faces)
            {
                Vector3 facePosition = face.position;
                
                if(max.X < facePosition.X)
                {
                    max.X = facePosition.X;
                }
                if(max.Y < facePosition.Y)
                {
                    max.Y = facePosition.Y;
                }
                if(max.Z < facePosition.Z)
                {
                    max.Z = facePosition.Z;
                }

                if(min.X > facePosition.X)
                {
                    min.X = facePosition.X;
                }
                if(min.Y > facePosition.Y)
                {
                    min.Y = facePosition.Y;
                }
                if(min.Z > facePosition.Z)
                {
                    min.Z = facePosition.Z;
                }
            }

            this.box = new BoundingBox(min, max);
            this.orignalBox = new BoundingBox(min, max);
        }

        private void reCalcTranslation()
        {
            for(int i = 0; i < faces.Length; i++)
            {
                faces[i].position = this._position + orignialFaces[i].position;
            }

            this.box = new BoundingBox(this.orignalBox.Min + this._position, this.orignalBox.Max + this._position);
        }

        private void reCalcRotation()
        {
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y, this._rotation.X, this._rotation.Z);

            for(int i = 0; i < faces.Length; i++)
            {
                faces[i].position = Vector3.Transform(orignialFaces[i].position, rotationMatrix) + this._position;

                faces[i].normal = Vector3.Transform(orignialFaces[i].normal, rotationMatrix);
            }

            this.localUp = Vector3.Transform(Vector3.Up, rotationMatrix);
            this.localForward = Vector3.Transform(Vector3.Forward, rotationMatrix);

            Vector3 max = faces[0].position;
            Vector3 min = faces[0].position;

            foreach(Face face in faces)
            {
                Vector3 facePosition = face.position;
                
                if(max.X < facePosition.X)
                {
                    max.X = facePosition.X;
                }
                if(max.Y < facePosition.Y)
                {
                    max.Y = facePosition.Y;
                }
                if(max.Z < facePosition.Z)
                {
                    max.Z = facePosition.Z;
                }

                if(min.X > facePosition.X)
                {
                    min.X = facePosition.X;
                }
                if(min.Y > facePosition.Y)
                {
                    min.Y = facePosition.Y;
                }
                if(min.Z > facePosition.Z)
                {
                    min.Z = facePosition.Z;
                }
            }

            this.box = new BoundingBox(min, max);
        }

        public float? rayIntersects(Ray ray) // todo: out Face faceHit
        {
            if(!ray.Intersects(this.box).HasValue) // if ray does not hit bounding box return
            {
                return null;
            }

            float? distance;
            float? shortestDistance = null;
            
            foreach(Face face in faces)
            {
                distance = ray.Intersects(face.plane);

                if(!distance.HasValue)
                {
                    continue;
                }

                Vector3 pointOnPlane = ray.Position + ray.Direction * distance.Value;

                Vector2 point;

                Vector3 localWorldUp = localUp;
                if(face.normal == localUp || face.normal == -localUp)
                {
                    localWorldUp = localForward;
                }

                Vector3 rightVector = Vector3.Normalize(Vector3.Cross(localWorldUp, face.normal));  
                Vector3 upVector = Vector3.Normalize(Vector3.Cross(rightVector, face.normal)); 

                point.X = Vector3.Dot(pointOnPlane - face.position, rightVector); 
                point.Y = Vector3.Dot(pointOnPlane - face.position, upVector);

                if(! IsInConvexPolygon(point, face.vertices))
                {
                    continue;
                }

                if(shortestDistance == null)
                {
                    shortestDistance = distance;
                }
                else if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                }
                
            }

            return shortestDistance;
        }

        // from ->  https://stackoverflow.com/questions/1119627/how-to-test-if-a-point-is-inside-of-a-convex-polygon-in-2d-integer-coordinates
        // user: Justas, edited by: Aiq0
        private static bool IsInConvexPolygon(Vector2 testPoint, Vector2[] polygon) 
        {   
            //Check if a triangle or higher n-gon
            Debug.Assert(polygon.Length >= 3);

            //n>2 Keep track of cross product sign changes
            int pos = 0;
            int neg = 0;

            for (var i = 0; i < polygon.Length; i++)
            {
                //If point is in the polygon
                if (polygon[i] == testPoint)
                    return true;

                //Form a segment between the i'th point
                float x1 = polygon[i].X;
                float y1 = polygon[i].Y;

                //And the i+1'th, or if i is the last, with the first point
                int i2 = (i+1)%polygon.Length;

                float x2 = polygon[i2].X;
                float y2 = polygon[i2].Y;

                float x = testPoint.X;
                float y = testPoint.Y;

                //Compute the cross product
                float d = (x - x1)*(y2 - y1) - (y - y1)*(x2 - x1);

                if (d > 0) pos++;
                if (d < 0) neg++;

                //If the sign changes, then point is outside
                if (pos > 0 && neg > 0)
                    return false;
            }

            //If no change in direction, then on same side of all segments, and thus inside
            return true;
        }   
    }

    public static class BasicBoundingMeshes
    {
        public static BoundingMesh BoundingRectangluarPrism(float width, float height, float depth, float scale, Vector3 position)
        {
            Face[] faces = new Face[6];

            faces[0] = new Face(0.5f * scale * width * Vector3.Right,    Vector3.Right,    depth * scale, height * scale);
            faces[1] = new Face(0.5f * scale * width * Vector3.Left,     Vector3.Left,     depth * scale, height * scale);
            faces[2] = new Face(0.5f * scale * height * Vector3.Up,      Vector3.Up,       width * scale, depth * scale);
            faces[3] = new Face(0.5f * scale * height * Vector3.Down,    Vector3.Down,     width * scale, depth * scale);
            faces[4] = new Face(0.5f * scale * depth * Vector3.Backward, Vector3.Backward, width * scale, height * scale);
            faces[5] = new Face(0.5f * scale * depth * Vector3.Forward,  Vector3.Forward,  width * scale, height * scale);

            return new BoundingMesh(faces, position, Vector3.Zero);
        }

        public static BoundingMesh BoundingCylinder(int sides, float height, float radius, float scale, Vector3 position) 
        {
            Face[] faces = new Face[2 + sides];

            faces[0] = new Face(0.5f * height * Vector3.Down, Vector3.Down, sides, radius);
            faces[1] = new Face(0.5f * height * Vector3.Up, Vector3.Up, sides, radius);

            for(int i = 2; i < sides + 2; i++)
            {
                float angle =  (MathHelper.TwoPi / sides) * i;
                float angle2 = (MathHelper.TwoPi / sides) * (i + 1);

                Vector3 pos1 = new Vector3(MathF.Cos(angle) * (radius), 0f, MathF.Sin(angle) * (radius));
                Vector3 pos2 = new Vector3(MathF.Cos(angle2) * (radius), 0f, MathF.Sin(angle2) * (radius));
                Vector3 center = (pos1 + pos2)/2f;

                float width = Vector3.Distance(pos2, pos1);

                faces[i] = new Face(center, Vector3.Normalize(center), width * scale, height * scale);
            }

            return new BoundingMesh(faces, position, Vector3.Zero);
        }

        public static BoundingMesh BoundingSphere()
        {
            throw new NotImplementedException();
            // Face[] faces = new Face[];


            // return new BoundingMesh(faces);
        }

        public static BoundingMesh BoundingTriangluarPrism(float length, float scale)
        {
            throw new NotImplementedException();

        //     Face[] faces = new Face[5];

        //     faces[0] = new Face(0.5f * length * Vector3.Forward, Vector3.Forward, 3, scale);
        //     faces[1] = new Face(-0.5f * length * Vector3.Backward, Vector3.Backward, 3, scale);

        //     faces[2] = new Face(Vector3.Zero, Vector3.Down, length, 1f);
        //     faces[3] = new Face(Vector3.Zero, Vector3.Down, length, 1f);
        //     faces[4] = new Face(Vector3.Zero, Vector3.Down, length, 1f);

        //     return new BoundingMesh(faces);
        }
    }
}   