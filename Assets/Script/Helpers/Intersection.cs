using UnityEngine;
using System.Collections;

public class Intersection {
	
	public class Triangle2
	{
		public Vector2 myLeftSide;
		public Vector2 myRightSide;
		public float myFarPlane;
	}

	[System.Serializable]
	public class FlatSphere
	{
		public FlatSphere(){}

		public FlatSphere(FlatSphere aFlatSphere)
		{
			myCenterPosition = aFlatSphere.myCenterPosition;
			myRadius = aFlatSphere.myRadius;
			myRadiusSquared = aFlatSphere.myRadiusSquared;
		}

		public FlatSphere(Vector2 pos, float radius)
		{
			myCenterPosition = pos;
			myRadius = radius;
			myRadiusSquared = radius*radius;
		}
		public Vector2 myCenterPosition;
		public float myRadius;
		public float myRadiusSquared;
		public bool myHaveBeenChecked;
	}

	[System.Serializable]
	public class Cube
	{
		public Cube(){}
		public Cube(Vector3 aPosition, Vector3 aSize)
		{
			center = aPosition;
			size = aSize;
		}

		public Vector3 center;
		public Vector3 size;
	}

	[System.Serializable]
	public class Box
	{
		public Box(){}
		public Box(Vector2 aPosition, Vector2 aSize)	
		{
			myPosition = aPosition;
			mySize = aSize;
		}
		public Vector2 myPosition;
		public Vector2 mySize;
	}
	
	public class Sphere
	{
		public Sphere(){}
		public Sphere(ref Sphere aSphere){myCenterPosition = aSphere.myCenterPosition; myRadius = aSphere.myRadius; myRadiusSquared = aSphere.myRadiusSquared;}
		public Sphere(Vector3 aCenterPos, float aRadius){myCenterPosition = aCenterPos; myRadius = aRadius; myRadiusSquared = aRadius * aRadius;}
		public void Update(float aDeltaTime)
		{
		}
		
		public Vector3 myCenterPosition;
		public float myRadius;
		public float myRadiusSquared;
	}
	
	public class LineSegment3D
	{
		public LineSegment3D(){}
		public LineSegment3D(ref LineSegment3D aLine){myStartPos = aLine.myStartPos; myEndPos = aLine.myEndPos;}
		public LineSegment3D(Vector3 aStartPos, Vector3 aEndPos){myStartPos = aStartPos; myEndPos = aEndPos;}
		public Vector3 myStartPos;
		public Vector3 myEndPos;

		public Vector3 GetDirection(){
			return (myEndPos-myStartPos).normalized;
		}
		public float GetLength(){
			return (myEndPos-myStartPos).magnitude;
		}
		public Vector3 GetAtDist(float targetDist){
			if(targetDist == 0)
				return myStartPos;
			return myStartPos + GetDirection() * targetDist;
		}

		public bool GetAtPlane(Intersection.Plane3D plane, out Vector3 returnVal){
			returnVal = myStartPos;
			LineSegment3D tempThis = this;
			if(Intersection.LineVsPlane(ref tempThis, plane, out returnVal, true))
				return true;
			return false;
		}
	}

	public class LineSegmentDist
	{
		public LineSegmentDist(){}
		public LineSegmentDist(ref LineSegmentDist aLine){myStartPos = aLine.myStartPos; myDirection = aLine.myDirection; distance = aLine.distance;}
		public LineSegmentDist(Vector3 aStartPos, Vector3 aDirection, float aDistancee){myStartPos = aStartPos; myDirection = aDirection; distance = aDistancee;}
		public Vector3 myStartPos;
		public Vector3 myDirection;
		public float distance;
	}
	[System.Serializable]
	public class LineSegment2D
	{
		public LineSegment2D()
		{
		}
		public LineSegment2D(Vector2 startPos, Vector2 endpos)
		{
			myStartPos = startPos;
			myEndPos = endpos;
		}
		public Vector2 myStartPos;
		public Vector2 myEndPos;
	}
	
	
	public class Plane
	{
		public Vector3 aNormal;
	}
	[System.Serializable]
	public class Plane3D
	{
		public Plane3D(){
		}

		public Plane3D(Vector3 pos, Vector3 normalIn){
			this.normal = normalIn;
			position = pos;
		}
		public Vector3 normal;
		public Vector3 position;
	}
	
	public class AABB
	{
		public Sphere GetSquaredSphere()
		{
			Sphere sphere = new Sphere();
			sphere.myCenterPosition = myCenterPos;
			sphere.myRadiusSquared = (myMaxPos-myMinPos).sqrMagnitude/2;
			return sphere;
		}
		public Plane3D[] GetPlanes()
		{
			Plane3D[] planes = new Plane3D[6];
			for(int index = 0; index < 6; index++)
				planes[index] = new Plane3D();
			planes[0].position = myCenterPos + new Vector3(myMinPos.x - myCenterPos.x,0,0);
			planes[1].position = myCenterPos + new Vector3(myMaxPos.x - myCenterPos.x,0,0);
			
			planes[2].position = myCenterPos + new Vector3(0, myMinPos.y - myCenterPos.y,0);
			planes[3].position = myCenterPos + new Vector3(0, myMaxPos.y - myCenterPos.y,0);

			
			planes[4].position = myCenterPos + new Vector3(0,0, myMinPos.z - myCenterPos.z);
			planes[5].position = myCenterPos + new Vector3(0,0, myMaxPos.z - myCenterPos.z);

			for(int index = 0; index < 6; index++)
				planes[index].normal = (myCenterPos - planes[index].position).normalized;

			return planes;
		}
		public Vector3 myMinPos;
		public Vector3 myMaxPos;
		public Vector3 myCenterPos;
		public Vector3 myExtents;
	}
	
	[System.Serializable]
	public class SwepthSphere
	{
		public SwepthSphere(){}
		public SwepthSphere(Vector3 start, Vector3 end, float rad)
		{
			myFirstPoint = start;
			mySecondPoint = end;
			myRadius = rad;
		}
		public Vector3 myFirstPoint;
		public Vector3 mySecondPoint;
		public float myRadius;
	}
	
	[System.Serializable]
	public class SwepthCircle
	{
		public SwepthCircle(){}
		public SwepthCircle(Vector2 start, Vector2 end, float rad)
		{
			myFirstPoint = start;
			mySecondPoint = end;
			myRadius = rad;
		}
		public Vector2 myFirstPoint;
		public Vector2 mySecondPoint;
		public float myRadius;
	}

	[System.Serializable]
	public class Triangle
	{
		public Vector3[] myPoints = new Vector3[3];
		public Vector3 GetNormal()
		{
			var dir = Vector3.Cross(myPoints[1] - myPoints[0], myPoints[2] - myPoints[0]);
			var norm = Vector3.Normalize(dir);
			return norm;
		}
		public Vector3 GetCenter()
		{
			return myPoints[0] * 0.333333333334f + myPoints[1] * 0.333333333334f + myPoints[2] * 0.333333333334f;
		}
	}
	public class Frustrum
	{
		public Vector3 myNearUpRight;
		public Vector3 myNearUpLeft;
		public Vector3 myNearDownRight;
		public Vector3 myNearDownLeft;
		
		public Vector3 myFarUpRight;
		public Vector3 myFarUpLeft;
		public Vector3 myFarDownRight;
		public Vector3 myFarDownLeft;
		
		public bool CheckInside(ref Sphere aSphere)
		{
			Debug.LogError("CheckInside has not been fully implemented");
			////nearPlane
			//Vector3 aPoint = aSphere.myCenterPosition;
			//aPoint = aSphere.myCenterPosition;
			//aPoint.y += aSphere.myRadius;
			//if(aPoint.y < myNearDownLeft.y)
			//{
			//	return false;
			//}
			//
			////FarPlane
			//aPoint = aSphere.myCenterPosition;
			//aPoint.y -= aSphere.myRadius;
			//if(aPoint.y > myFarDownLeft.y)
			//{
			//	return false;
			//}
			//
			////LeftSide
			//
			//Plane<float> aPlane;
			//aPlane.InitWith3Points(myNearDownLeft, myNearUpLeft, myFarDownLeft);
			//aPoint = aSphere.myCenterPosition;
			//aPoint.x += aSphere.myRadius;
			//if(aPlane.Inside(aPoint) == false)
			//{
			//	return false;
			//}
			//
			////RightSide
			//aPlane.InitWith3Points(myNearDownRight, myFarDownRight, myNearUpRight);
			//aPoint = aSphere.myCenterPosition;
			//aPoint.x -= aSphere.myRadius;
			//if(aPlane.Inside(aPoint) == false)
			//{
			//	return false;
			//}
			//
			////TopSide
			//aPlane.InitWith3Points(myNearDownRight, myNearDownLeft, myFarDownRight);
			//aPoint = aSphere.myCenterPosition;
			//aPoint.y += aSphere.myRadius;
			//if(aPlane.Inside(aPoint) == false)
			//{
			//	return false;
			//}
			//
			////BotSide
			//aPlane.InitWith3Points(myNearUpRight, myFarUpRight, myNearUpLeft);
			//aPoint = aSphere.myCenterPosition;
			//aPoint.y -= aSphere.myRadius;
			//if(aPlane.Inside(aPoint) == false)
			//{
			//	return false;
			//}
			return true;
		}
	}
	public class Fov90Frustrum
	{
		public Fov90Frustrum(float aNearPlane, float aFarPlane)
		{
			myNearPlane = aNearPlane;
			myFarPlane = aFarPlane;
		}
		public float myNearPlane, myFarPlane;
	}
	
	public static bool PointInsideFlatSphere(ref FlatSphere aSphere, ref Vector2 aTestPoint)
	{
		float distance = (aTestPoint - aSphere.myCenterPosition).magnitude;
		if(distance < aSphere.myRadius){
			aTestPoint = aSphere.myRadius * (aTestPoint - aSphere.myCenterPosition).normalized + aSphere.myCenterPosition;
			return true;
		}

		return false;
	}

	public static bool FlatSphereVsFlatSphere(FlatSphere aSphere, FlatSphere aSecondSphere){
		Vector2 dif = aSphere.myCenterPosition - aSecondSphere.myCenterPosition;
		return dif.magnitude < (aSphere.myRadius + aSecondSphere.myRadius);
	}

	public static bool PointInsideSphere(ref Sphere aSphere, ref Vector3 aTestPoint)
	{
		Vector3 aVector = aSphere.myCenterPosition - aTestPoint;
		float aLength = aVector.sqrMagnitude;
		if(aLength > -(aSphere.myRadius*aSphere.myRadius) && aLength < aSphere.myRadius*aSphere.myRadius)
		{
			return true;
		}
		return false;
	}
	public static bool PointInsideAABB(ref AABB anAABB, ref Vector3 aTestPoint)
	{
		if(anAABB.myMaxPos.x >= aTestPoint.x)
		{
			if(anAABB.myMinPos.x <= aTestPoint.x)
			{
				if(anAABB.myMaxPos.y >= aTestPoint.y)
				{
					if(anAABB.myMinPos.y <= aTestPoint.y)
					{
						if(anAABB.myMaxPos.z >= aTestPoint.z)
						{
							if(anAABB.myMinPos.z <= aTestPoint.z)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public static Vector3 GetClosestPointOnPlane(Vector3 point, Plane3D plane){
		Vector3 dir = point - plane.position;
		plane.normal = plane.normal.normalized;
		float dotVal = Vector3.Dot(dir.normalized, plane.normal);
		
		return point + plane.normal * dir.magnitude * (-dotVal);
	}

	public static bool PointInsidePlane(Vector3 point, Plane3D plane, out Vector3 intersectionPoint){
		Vector3 dir = point - plane.position;
		plane.normal = plane.normal.normalized;
		float dotVal = Vector3.Dot(dir.normalized, plane.normal);
		if(dotVal < 0)
			intersectionPoint = point + plane.normal * dir.magnitude * (-dotVal);
		else
			intersectionPoint = point;
		return (dotVal > 0);
	}

	public static bool LineVsPlane(ref LineSegmentDist line, Plane3D plane, out Vector3 intersectionPoint, bool allowInverse = false) 
	{
		float normalDot = -Vector3.Dot(plane.normal, line.myDirection);
		if(normalDot <= 0.0f && !allowInverse)
		{
			intersectionPoint = Vector3.zero;
			return false;
		}
		float distance = -(Vector3.Dot(plane.normal,  plane.position) -Vector3.Dot(plane.normal, line.myStartPos)) / normalDot;

		intersectionPoint = line.myStartPos + line.myDirection * distance;
		return distance <= line.distance;
	}


	public static bool LineVsPlane(ref LineSegment3D line, Plane3D plane, out Vector3 intersectionPoint, bool allowInverse = false) 
	{
		Vector3 direction = line.myEndPos - line.myStartPos;
		Vector3 normalizedDirection = direction.normalized;
		float normalDot = Vector3.Dot(plane.normal, normalizedDirection);
		if(normalDot <= 0.0f && !allowInverse)
		{
			intersectionPoint = Vector3.zero;
			return false;
		}
		Vector3 directionToPlane = (plane.position - line.myStartPos);
		Vector3 directionToPlaneNormalized = directionToPlane.normalized;
		float dotToPlanePoint = Vector3.Dot(directionToPlaneNormalized, plane.normal);
		float distance = (plane.position - line.myStartPos).magnitude;
		Vector3 closestPointOnPlane = line.myStartPos + plane.normal * (distance*dotToPlanePoint);

		float distanceFromClosestPoint = (closestPointOnPlane - line.myStartPos).magnitude;
		float distanceToPlane = distanceFromClosestPoint / (normalDot);
		//float distance = -(Vector3.Dot(plane.normal,  plane.position) -Vector3.Dot(plane.normal, line.myStartPos)) / normalDot;
		if(distanceToPlane > direction.magnitude){
			intersectionPoint = Vector3.zero;
			return false;
		}
		intersectionPoint = line.myStartPos + normalizedDirection * distanceToPlane;
		//intersectionPoint = closestPointOnPlane;
		return true;
	}

	public static bool LineVsLine(ref LineSegment2D aFirstLine, ref LineSegment2D aSecondLine, ref Vector2 anIntersectionPoint)
	{

		Vector2 inA = aFirstLine.myStartPos;
		Vector2 inB = aFirstLine.myEndPos;
		Vector2 inC = aSecondLine.myStartPos;
		Vector2 inD = aSecondLine.myEndPos;

		Vector2 r = (inB - inA);
		Vector2 s = (inD - inC);

		float d = r.x * s.y - r.y * s.x; 
		float u = ((inC.x - inA.x) * r.y - (inC.y - inA.y) * r.x) / d;
		float t = ((inC.x - inA.x) * s.y - (inC.y - inA.y) * s.x) / d;
		anIntersectionPoint = inA + t * r;
		return (0 <= u && u <= 1 && 0 <= t && t <= 1);
		//Vector2 aLineSegmentStart = aSecondLine.myStartPos;
		//Vector2 aLineSegmentEnd = aSecondLine.myEndPos;
		//Vector2 aSecondLineStart = aFirstLine.myStartPos;
		//Vector2 aSecondLineEnd = aFirstLine.myEndPos;
		//Vector3 secondLineDir = new Vector3(aSecondLineEnd.x, aSecondLineEnd.y, 0).normalized;
		//
		//Vector2 firstLineDir2 = (aLineSegmentStart - aLineSegmentEnd).normalized;
		//Vector2 secondLineDir2 = (aSecondLineStart - aSecondLineEnd).normalized;
		//
		//float angle = Vector2.Dot(firstLineDir2, secondLineDir2);
		//
		//if(angle == 1 || angle == -1)
		//	return false;//Check if parralel
		//
		//float firstLineSize = (aLineSegmentEnd - aLineSegmentStart).magnitude;
		//float secondLineSize = (aSecondLineStart - aSecondLineEnd).magnitude;

		//float angle = Vector3.Dot(cross, cross);
		//Vector3 r = Vector3.Cross(temp, cross / angle);
		//
		////float distanceToClosestPointOnLineOne = Vector3.Dot(r, lineSegmentDirection);
		//float distanceToClosestPointOnLineTwo = Vector3.Dot(r, secondLineDir);
		//
		////Vector3 closestPointOnInfLine = new Vector3(aSecondLineStart.x, aSecondLineStart.y, 0) + secondLineDir * distanceToClosestPointOnLineOne;
		//Vector3 closestPointOnLineSegment = new Vector3(aLineSegmentStart.x, aLineSegmentStart.y, 0) + lineSegmentDirection * distanceToClosestPointOnLineTwo;
		//
		//Vector2 intersectionPoint = new Vector2(closestPointOnLineSegment.x, closestPointOnLineSegment.y);
		//
		//if((intersectionPoint - aLineSegmentEnd).magnitude < firstLineSize){
		//	if((intersectionPoint - aLineSegmentStart).magnitude < firstLineSize){
		//		if((intersectionPoint - aSecondLineStart).magnitude < secondLineSize){
		//			if((intersectionPoint - aSecondLineEnd).magnitude < secondLineSize){
		//				anIntersectionPoint = intersectionPoint;
		//				return true;
		//			}
		//		}
		//	}
		//}
		return false;
	}
	public static bool InfLineVSLineSegment(ref LineSegment2D aFirstLine, ref LineSegment2D aSecondLine, ref Vector2 anIntersectionPoint)
	{
		float tempA;
		float tempB;
		float denominator;
		float tempANumerator;
		float tempBNumerator;
		
		denominator = ((aSecondLine.myEndPos.y - aSecondLine.myStartPos.y) * (aFirstLine.myEndPos.x - aFirstLine.myStartPos.x) - (aSecondLine.myEndPos.x - aSecondLine.myStartPos.x) * (aFirstLine.myEndPos.y - aFirstLine.myStartPos.y));
		tempANumerator = ((aSecondLine.myEndPos.x - aSecondLine.myStartPos.x) * (aFirstLine.myStartPos.y - aSecondLine.myStartPos.y) - (aSecondLine.myEndPos.y - aSecondLine.myStartPos.y) * (aFirstLine.myStartPos.x - aSecondLine.myStartPos.x));
		tempBNumerator = ((aFirstLine.myEndPos.x - aFirstLine.myStartPos.x) * (aFirstLine.myStartPos.y - aSecondLine.myStartPos.y) - (aFirstLine.myEndPos.y - aFirstLine.myStartPos.y) * (aFirstLine.myStartPos.x - aSecondLine.myStartPos.x));
		
		
		if(denominator == 0)
		{
			if(tempANumerator == 0 && tempBNumerator == 0)
			{
				return (true);
			}
			else
			{
				return (false);
			}
		}
		
		tempA = tempANumerator / denominator;
		tempB = tempBNumerator / denominator;
		
		if(tempA >= 0 && tempA <= 1)
		{
			if(tempB >= 0 && tempB <= 1)
			{
				anIntersectionPoint.x = aFirstLine.myStartPos.x + tempA*(aFirstLine.myEndPos.x - aFirstLine.myStartPos.x);
				anIntersectionPoint.y = aFirstLine.myStartPos.y + tempA*(aFirstLine.myEndPos.y - aFirstLine.myStartPos.y);
				return (true);
			}
		}
		
		
		
		return (false);
		
	}

	public static bool RayVsSphere(Ray aLine, ref Sphere aSphere, ref Vector3 anIntersectionPoint){
		Vector3 closestPoint = GetClosestPointInRay(aLine, aSphere.myCenterPosition);
		
		if(PointInsideSphere(ref aSphere, ref closestPoint)){
			anIntersectionPoint = closestPoint;
			return true;
		}
		return false;
	}

	public static bool LineVsSphere(ref LineSegment3D aLine, ref Sphere aSphere, ref Vector3 anIntersectionPoint)
	{   
		Vector3 closestPoint = GetClosestPoint(aLine.myStartPos, aLine.myEndPos, aSphere.myCenterPosition);
		
		if(PointInsideSphere(ref aSphere, ref closestPoint) == true)
		{
			anIntersectionPoint = closestPoint;
			return true;
		}
		return false;
	}
	public static float ClosestPointsOnTwoLines( LineSegment3D aFirstLine, LineSegment3D aSecondLine, out Vector3 aClosestPointOnFirstLine, out Vector3 aClosestPointOnSecondLine)
	{
		aClosestPointOnSecondLine = Vector3.zero;
		aClosestPointOnFirstLine = Vector3.zero;
		float SMALL_NUM = 0.0000001f;//float.Epsilon
		Vector3 firstLineDir = aFirstLine.myEndPos - aFirstLine.myStartPos;
		Vector3 secondLineDir = aSecondLine.myEndPos - aSecondLine.myStartPos;
		Vector3 directionBetweenLines = aFirstLine.myStartPos - aSecondLine.myStartPos;
		float a = Vector3.Dot(firstLineDir, firstLineDir);         // always >= 0
		float b = Vector3.Dot(firstLineDir, secondLineDir);
		float c = Vector3.Dot(secondLineDir, secondLineDir);         // always >= 0
		float d = Vector3.Dot(firstLineDir, directionBetweenLines);
		float e = Vector3.Dot(secondLineDir, directionBetweenLines);
		float D = a*c - b*b;        // always >= 0
		float sc, sN, sD = D;       // sc = sN / sD, default sD = D >= 0
		float tc, tN, tD = D;       // tc = tN / tD, default tD = D >= 0
		
		// compute the line parameters of the two closest points
		if (D < SMALL_NUM) { // the lines are almost parallel
			sN = 0.0f;         // force using point P0 on segment S1
			sD = 1.0f;         // to prevent possible division by 0.0 later
			tN = e;
			tD = c;
		}
		else {                 // get the closest points on the infinite lines
			sN = (b*e - c*d);
			tN = (a*e - b*d);
			if (sN < 0.0f) {        // sc < 0 => the s=0 edge is visible
				sN = 0.0f;
				tN = e;
				tD = c;
			}
			else if (sN > sD) {  // sc > 1  => the s=1 edge is visible
				sN = sD;
				tN = e + b;
				tD = c;
			}
		}
		
		if (tN < 0.0f) {            // tc < 0 => the t=0 edge is visible
			tN = 0.0f;
			// recompute sc for this edge
			if (-d < 0.0f)
				sN = 0.0f;
			else if (-d > a)
				sN = sD;
			else {
				sN = -d;
				sD = a;
			}
		}
		else if (tN > tD) {      // tc > 1  => the t=1 edge is visible
			tN = tD;
			// recompute sc for this edge
			if ((-d + b) < 0.0f)
				sN = 0;
			else if ((-d + b) > a)
				sN = sD;
			else {
				sN = (-d +  b);
				sD = a;
			}
		}
		// finally do the division to get sc and tc
		sc = (float)((Mathf.Abs(sN) < SMALL_NUM ? 0.0f : sN / sD));
		tc = (float)((Mathf.Abs(tN) < SMALL_NUM ? 0.0f : tN / tD));
		
		// get the difference of the two closest points
		Vector3 dP = directionBetweenLines + (firstLineDir * sc) - (secondLineDir * tc);  // =  S1(sc) - S2(tc)
		aClosestPointOnFirstLine = (firstLineDir * sc) + aFirstLine.myStartPos;
		aClosestPointOnSecondLine = (secondLineDir * tc) + aSecondLine.myStartPos;
		
		return dP.magnitude;   // return the closest distance
	}
	
	public static float ClosestPointsOnTwoLines(LineSegment2D aFirstLine, LineSegment2D aSecondLine, out Vector2 aClosestPointOnFirstLine, out Vector2 aClosestPointOnSecondLine)
	{
		aClosestPointOnSecondLine = Vector2.zero;
		aClosestPointOnFirstLine = Vector2.zero;
		float SMALL_NUM = 0.0000001f;//float.Epsilon
		Vector2 firstLineDir = aFirstLine.myEndPos - aFirstLine.myStartPos;
		Vector2 secondLineDir = aSecondLine.myEndPos - aSecondLine.myStartPos;
		Vector2 directionBetweenLines = aFirstLine.myStartPos - aSecondLine.myStartPos;
		float a = Vector2.Dot(firstLineDir, firstLineDir);         // always >= 0
		float b = Vector2.Dot(firstLineDir, secondLineDir);
		float c = Vector2.Dot(secondLineDir, secondLineDir);         // always >= 0
		float d = Vector2.Dot(firstLineDir, directionBetweenLines);
		float e = Vector2.Dot(secondLineDir, directionBetweenLines);
		float D = a*c - b*b;        // always >= 0
		float sc, sN, sD = D;       // sc = sN / sD, default sD = D >= 0
		float tc, tN, tD = D;       // tc = tN / tD, default tD = D >= 0
		
		// compute the line parameters of the two closest points
		if (D < SMALL_NUM) { // the lines are almost parallel
			sN = 0.0f;         // force using point P0 on segment S1
			sD = 1.0f;         // to prevent possible division by 0.0 later
			tN = e;
			tD = c;
		}
		else {                 // get the closest points on the infinite lines
			sN = (b*e - c*d);
			tN = (a*e - b*d);
			if (sN < 0.0f) {        // sc < 0 => the s=0 edge is visible
				sN = 0.0f;
				tN = e;
				tD = c;
			}
			else if (sN > sD) {  // sc > 1  => the s=1 edge is visible
				sN = sD;
				tN = e + b;
				tD = c;
			}
		}
		
		if (tN < 0.0f) {            // tc < 0 => the t=0 edge is visible
			tN = 0.0f;
			// recompute sc for this edge
			if (-d < 0.0f)
				sN = 0.0f;
			else if (-d > a)
				sN = sD;
			else {
				sN = -d;
				sD = a;
			}
		}
		else if (tN > tD) {      // tc > 1  => the t=1 edge is visible
			tN = tD;
			// recompute sc for this edge
			if ((-d + b) < 0.0f)
				sN = 0;
			else if ((-d + b) > a)
				sN = sD;
			else {
				sN = (-d +  b);
				sD = a;
			}
		}
		// finally do the division to get sc and tc
		sc = (float)((Mathf.Abs(sN) < SMALL_NUM ? 0.0f : sN / sD));
		tc = (float)((Mathf.Abs(tN) < SMALL_NUM ? 0.0f : tN / tD));
		
		// get the difference of the two closest points
		Vector2 dP = directionBetweenLines + (firstLineDir * sc) - (secondLineDir * tc);  // =  S1(sc) - S2(tc)
		aClosestPointOnFirstLine = (firstLineDir * sc) + aFirstLine.myStartPos;
		aClosestPointOnSecondLine = (secondLineDir * tc) + aSecondLine.myStartPos;
		
		return dP.magnitude;   // return the closest distance
	}
	public static bool LineVsSwepthSphere(ref LineSegment3D aLine, ref SwepthSphere aSphere, ref Vector3 anIntersectionPoint)
	{
		LineSegment3D sphereLine = new LineSegment3D(aSphere.myFirstPoint, aSphere.mySecondPoint);
		
		Vector3 closestPointOnLine = Vector3.zero;
		Vector3 closestPointInSwepthSphere = Vector3.zero;
		float distance = ClosestPointsOnTwoLines(aLine, sphereLine, out closestPointOnLine, out closestPointInSwepthSphere);
		
		if(distance > -aSphere.myRadius && distance < aSphere.myRadius )
		{
			return true;
		}
		
		Sphere sphere = new Sphere(closestPointInSwepthSphere, aSphere.myRadius);
		return PointInsideSphere(ref sphere, ref closestPointOnLine);
	}
	public static bool PointInsideSwepthSphere2D(Vector2 start, Vector2 end, Vector2 point, float radius, ref float distance)
	{
		Vector2 closest = point.GetClosest(start, end);
		distance = (closest - point).sqrMagnitude;
		
		if(distance < radius * radius)
		{
			return true;
		}
		return false;
	}
	public static bool PointInsideSwepthSphere2D(Vector2 start, Vector2 end, Vector2 point, float radius, ref Vector2 intersectionPoint)
	{
		Vector2 closest = point.GetClosest(start, end);
		Vector2 dir = (closest - point);
		float distance = dir.magnitude;
		
		if(distance < radius)
		{
			intersectionPoint = point - dir.normalized * (radius-distance);
			return true;
		}
		intersectionPoint = point;
		return false;
	}
	public static bool LineVsAABB(ref LineSegment3D aLine, ref AABB anAABB, ref Vector3 anIntersectionPoint)
	{
		bool inside = true;
		
		Vector3 rayOrg = aLine.myStartPos;
		Vector3 rayDelta = aLine.myEndPos - aLine.myStartPos;
		
		Vector3 min = anAABB.myMinPos;
		Vector3 max = anAABB.myMaxPos;
		
		float xt, xn;
		
		if( rayOrg.x < min.x )
		{
			xt = min.x - rayOrg.x;
			if( xt > rayDelta.x )
			{
				return false;
			}
			xt /= rayDelta.x;
			inside = false;
			xn = -1.0f;
		}
		else if( rayOrg.x > max.x )
		{
			xt = max.x - rayOrg.x;
			if( xt < rayDelta.x )
			{
				return false;
			}
			xt /= rayDelta.x;
			inside = false;
			xn = 1.0f;
		}
		else
		{
			xt = -1.0f;
		}
		
		float yt, yn;
		
		if( rayOrg.y < min.y )
		{
			yt = min.y - rayOrg.y;
			if( yt > rayDelta.y )
			{
				return false;
			}
			yt /= rayDelta.y;
			inside = false;
			yn = -1.0f;
		}
		else if( rayOrg.y > max.y )
		{
			yt = max.y - rayOrg.y;
			if( yt < rayDelta.y )
			{
				return false;
			}
			yt /= rayDelta.y;
			inside = false;
			yn = 1.0f;
		}
		else
		{
			yt = -1.0f;
		}
		
		float zt, zn;
		
		if( rayOrg.z < min.z )
		{
			zt = min.z - rayOrg.z;
			if( zt > rayDelta.z )
			{
				return false;
			}
			zt /= rayDelta.z;
			inside = false;
			zn = -1.0f;
		}
		else if( rayOrg.z > max.z )
		{
			zt = max.z - rayOrg.z;
			if( zt < rayDelta.z )
			{
				return false;
			}
			zt /= rayDelta.z;
			inside = false;
			zn = 1.0f;
		}
		else
		{
			zt = -1.0f;
		}
		
		if( inside == true )
		{
			Vector3 temp = Vector3.zero;
			temp -= rayDelta;
			anIntersectionPoint = temp;
			return true;
		}
		
		int which = 0;
		float t = xt;
		if( yt > t )
		{
			which = 1;
			t = yt;
		}
		
		if( zt > t )
		{
			which = 2;
			t = zt;
		}
		
		switch( which )
		{
		case 0:
		{
			float y = rayOrg.y + rayDelta.y * t;
			if( y < min.y || y > max.y )
			{
				return false;
			}
			float z = rayOrg.z + rayDelta.z * t;
			if( z < min.z || z > max.z )
			{
				return false;
			}
			
			anIntersectionPoint = rayOrg + rayDelta * t;
			
			break;
		}
		case 1:
		{
			float x = rayOrg.x + rayDelta.x * t;
			if( x < min.x || x > max.x )
			{
				return false;
			}
			float z = rayOrg.z + rayDelta.z * t;
			if( z < min.z || z > max.z )
			{
				return false;
			}
			
			anIntersectionPoint = rayOrg + rayDelta * t;
			
			break;
		}
		case 2:
		{
			float x = rayOrg.x + rayDelta.x * t;
			if( x < min.x || x > max.x )
			{
				return false;
			}
				float y = rayOrg.y + rayDelta.y * t;
				if( y < min.y || y > max.y )
				{
					return false;
				}
				
				anIntersectionPoint = rayOrg + rayDelta * t;
				
				break;
			}
		}
		
		return true;
	}
	
	public static bool PointInsideBox(Box aBox, Vector2 aPoint)
	{
		if(aPoint.x <= aBox.myPosition.x)
		{
			return false;
		}
		if(aPoint.y <= aBox.myPosition.y)
		{
			return false;
		}
		
		if(aPoint.x >= aBox.myPosition.x + aBox.mySize.x)
		{
			return false;
		}
		if(aPoint.y >= aBox.myPosition.y + aBox.mySize.y)
		{
			return false;
		}
		return true;
	}
	public static bool BoxVsBox(ref Box aBox, ref Box aSecondBox)
	{
		if(aSecondBox.myPosition.x + aSecondBox.mySize.x <= aBox.myPosition.x)
		{
			return false;
		}
		if(aSecondBox.myPosition.y + aSecondBox.mySize.y <= aBox.myPosition.y)
		{
			return false;
		}
		
		if(aSecondBox.myPosition.x >= aBox.myPosition.x + aBox.mySize.x)
		{
			return false;
		}
		if(aSecondBox.myPosition.y >= aBox.myPosition.y + aBox.mySize.y)
		{
			return false;
		}
		return true;
	}
	public static bool Sphere2VsBox(ref FlatSphere aSphere, ref Box aBox)
	{
		Sphere aSecondSphere = new Sphere();
		aSecondSphere.myCenterPosition = aSphere.myCenterPosition;
		aSecondSphere.myRadius = aSphere.myRadius;
		aSecondSphere.myRadiusSquared= aSphere.myRadiusSquared;
		
		AABB aAABB = new AABB();
		aAABB.myMinPos = aBox.myPosition;
		aAABB.myMaxPos = aBox.mySize+aBox.myPosition;
		aAABB.myCenterPos = aBox.myPosition+(aBox.mySize/2);
		Vector3 intersectionPoint;
		return SphereVsAABB(aSecondSphere, aAABB, out intersectionPoint);
	}
	public static bool SphereVsSphere(Sphere aFirstSphere, Sphere aSecondSphere)
	{
		Vector3 aVector = aSecondSphere.myCenterPosition - aFirstSphere.myCenterPosition;
		float aLenth = aVector.sqrMagnitude;
		float totalRadius = aFirstSphere.myRadius + aSecondSphere.myRadius;
		totalRadius *= totalRadius;
		if(aLenth < totalRadius)
		{
			return true;
		}
		return false;
	}
	public static bool SphereVsAABB(Sphere aFirstSphere, AABB anAABB, out Vector3 aIntersectionPoint)
	{
		aIntersectionPoint = Vector3.zero;
		if(anAABB.myMaxPos.x >= aFirstSphere.myCenterPosition.x - aFirstSphere.myRadius)
		{
			if(anAABB.myMinPos.x <= aFirstSphere.myCenterPosition.x + aFirstSphere.myRadius)
			{
				if(anAABB.myMaxPos.y >= aFirstSphere.myCenterPosition.y - aFirstSphere.myRadius)
				{
					if(anAABB.myMinPos.y <= aFirstSphere.myCenterPosition.y + aFirstSphere.myRadius)
					{
						if(anAABB.myMaxPos.z >= aFirstSphere.myCenterPosition.z - aFirstSphere.myRadius)
						{
							if(anAABB.myMinPos.z <= aFirstSphere.myCenterPosition.z + aFirstSphere.myRadius)
							{
								aIntersectionPoint = aFirstSphere.myCenterPosition;
								return true;
							}
						}
					}
				}
			}
		}
		
		
		if(PointInsideAABB(ref anAABB, ref aFirstSphere.myCenterPosition) == true)
		{
			aIntersectionPoint = aFirstSphere.myCenterPosition;
			return true;
		}
		
		Vector3 aVector;
		aVector = anAABB.myMaxPos;
		if(PointInsideSphere(ref aFirstSphere, ref aVector) == true)
		{
			aIntersectionPoint = aVector;
			return true;
		}
		
		aVector = anAABB.myMaxPos;
		aVector.x = anAABB.myMinPos.x;
		if(PointInsideSphere(ref aFirstSphere, ref aVector) == true)
		{
			aIntersectionPoint = aVector;
			return true;
		}
		
		aVector = anAABB.myMaxPos;
		aVector.y = anAABB.myMinPos.y;
		if(PointInsideSphere(ref aFirstSphere, ref aVector) == true)
		{
			aIntersectionPoint = aVector;
			return true;
		}
		
		aVector = anAABB.myMaxPos;
		aVector.z = anAABB.myMinPos.z;
		if(PointInsideSphere(ref aFirstSphere, ref aVector) == true)
		{
			aIntersectionPoint = aVector;
			return true;
		}
		
		aVector = anAABB.myMinPos;
		if(PointInsideSphere(ref aFirstSphere, ref aVector) == true)
		{
			aIntersectionPoint = aVector;
			return true;
		}
		
		aVector = anAABB.myMinPos;
		aVector.x = anAABB.myMinPos.x;
		if(PointInsideSphere(ref aFirstSphere, ref aVector) == true)
		{
			aIntersectionPoint = aVector;
			return true;
		}
		
		aVector = anAABB.myMinPos;
		aVector.y = anAABB.myMinPos.y;
		if(PointInsideSphere(ref aFirstSphere, ref aVector) == true)
		{
			aIntersectionPoint = aVector;
			return true;
		}
		
		aVector = anAABB.myMinPos;
		aVector.z = anAABB.myMinPos.z;
		if(PointInsideSphere(ref aFirstSphere, ref aVector) == true)
		{
			aIntersectionPoint = aVector;
			return true;
		}
		
		return false;
	}
	public static bool AABBVsAABB(AABB aFirstAABB, AABB aSecondAABB)
	{
		if(aFirstAABB.myMaxPos.x < aSecondAABB.myMinPos.x && aFirstAABB.myMaxPos.y < aSecondAABB.myMinPos.y && aFirstAABB.myMaxPos.z < aSecondAABB.myMinPos.z)
		{
			return false;
		}
		
		AABB aBox = new AABB();
		aBox.myMaxPos = aFirstAABB.myMaxPos + aSecondAABB.myMaxPos;
		aBox.myMinPos = aFirstAABB.myMinPos - aSecondAABB.myMinPos;
		if(PointInsideAABB(ref aBox, ref aSecondAABB.myCenterPos) == true)
		{
			return true;
		}
		
		aBox.myMaxPos = aSecondAABB.myMaxPos + aFirstAABB.myMaxPos;
		aBox.myMinPos = aSecondAABB.myMinPos - aFirstAABB.myMinPos;
		if(PointInsideAABB(ref aBox , ref aFirstAABB.myCenterPos) == true)
		{
			return true;
		}
		return false;
	}
	
	
	public static bool LineVsTriangle(LineSegment3D aLine, Triangle aTriangle, out Vector3 anIntersectionPoint)
	{
		anIntersectionPoint = aLine.myStartPos;
		Vector3 triangleNormal = aTriangle.GetNormal();
		Vector3 triangleCenter = aTriangle.GetCenter();

		Plane3D tempPlane = new Plane3D();
		tempPlane.position = triangleCenter;
		tempPlane.normal = triangleNormal;
		Vector3 pointOnTriangle;
		if(!LineVsPlane(ref aLine, tempPlane, out pointOnTriangle))
			return false;
		
		//anIntersectionPoint = triangleCenter;
		Vector3 closestPoint = GetClosestPoint(aLine.myStartPos, aLine.myEndPos, triangleCenter);

		Vector3 closestPointOnTriangle = Vector3.zero;

		//float distance1 = (closestPoint - aTriangle.myPoints[0]).sqrMagnitude;
		//float distance2 = (closestPoint - aTriangle.myPoints[1]).sqrMagnitude;
		//float distance3 = (closestPoint - aTriangle.myPoints[2]).sqrMagnitude;
		//if(distance1 < distance3 && distance2 < distance3)
		//	ClosestPointsOnTwoLines(aLine, new LineSegment3D(aTriangle.myPoints[0], aTriangle.myPoints[1]), out closestPointOnTriangle, out closestPoint);
		//else if(distance2 < distance1 && distance3 < distance1)
		//	ClosestPointsOnTwoLines(aLine, new LineSegment3D(aTriangle.myPoints[1], aTriangle.myPoints[2]), out closestPointOnTriangle, out closestPoint);
		//else
		//	ClosestPointsOnTwoLines(aLine, new LineSegment3D(aTriangle.myPoints[0], aTriangle.myPoints[2]), out closestPointOnTriangle, out closestPoint);
		//
		//float edgeToPointAngle = Vector3.Dot((closestPointOnTriangle-closestPoint).normalized, (triangleCenter - closestPoint).normalized);
		//if(edgeToPointAngle < 0)
		//	return false;
		int falseCount = 0;
		Vector3 lineDirection = (aLine.myEndPos - aLine.myStartPos).normalized;
		for(int index = 0; index < aTriangle.myPoints.Length; index++)
		{
			Vector3 centerEdge = Vector3.Lerp(aTriangle.myPoints[index], aTriangle.myPoints[(index+1 >= aTriangle.myPoints.Length ? 0 : index+1)], 0.5f);
			anIntersectionPoint = pointOnTriangle;
			Vector3 normal = (triangleCenter - centerEdge).normalized;
			Vector3 normalToPoint = (pointOnTriangle - centerEdge).normalized;
			float dotVal = Vector3.Dot(normal, normalToPoint);
			if(dotVal < 0.0f)
				falseCount++;
		}

		return falseCount == 0 || falseCount == 3;
	}

	public static bool SwepthSphereVsSphere(SwepthSphere aSwepthSphere, Sphere aSphere, out Vector3 point)
	{
		Vector3 closestPont = GetClosestPoint( aSwepthSphere.myFirstPoint, aSwepthSphere.mySecondPoint, aSphere.myCenterPosition);
		
		float distance = (closestPont-aSphere.myCenterPosition).magnitude;
		point = closestPont + ((closestPont-aSphere.myCenterPosition)/distance) * aSwepthSphere.myRadius;
		return  distance <  aSphere.myRadius + aSwepthSphere.myRadius;
	}
	public static bool SwepthSphereVsSwepthSphere(SwepthSphere aSwepthSphere, SwepthSphere aSecondSwepthSphere, out Vector3 point)
	{
		point = Vector3.zero;
		Vector3 closestPoint = Vector3.zero, secondClosestPoint = Vector3.zero;
		LineSegment3D firstLine = new LineSegment3D(aSwepthSphere.myFirstPoint, aSwepthSphere.mySecondPoint);
		LineSegment3D secondLine = new LineSegment3D(aSecondSwepthSphere.myFirstPoint, aSecondSwepthSphere.mySecondPoint);
		ClosestPointsOnTwoLines(firstLine, secondLine, out closestPoint, out secondClosestPoint);
		Sphere firstSphere = new Sphere(closestPoint, aSwepthSphere.myRadius), secondSphere = new Sphere(secondClosestPoint, aSecondSwepthSphere.myRadius);
		if(SphereVsSphere(firstSphere, secondSphere))
		{
			point = firstSphere.myCenterPosition + (firstSphere.myCenterPosition-secondSphere.myCenterPosition).normalized * firstSphere.myRadius;
			return true;
		}
		return false;
	}

	public static bool SwepthCircleVsSwepthCircle(SwepthCircle aSwepthSphere, SwepthCircle aSecondSwepthSphere, out Vector2 point)
	{
		point = Vector2.zero;
		Vector2 closestPoint = Vector2.zero, secondClosestPoint = Vector2.zero;
		LineSegment2D firstLine = new LineSegment2D(aSwepthSphere.myFirstPoint, aSwepthSphere.mySecondPoint);
		LineSegment2D secondLine = new LineSegment2D(aSecondSwepthSphere.myFirstPoint, aSecondSwepthSphere.mySecondPoint);
		ClosestPointsOnTwoLines(firstLine, secondLine, out closestPoint, out secondClosestPoint);
		FlatSphere firstSphere = new FlatSphere(closestPoint, aSwepthSphere.myRadius), secondSphere = new FlatSphere(secondClosestPoint, aSecondSwepthSphere.myRadius);
		if(FlatSphereVsFlatSphere(firstSphere, secondSphere))
		{
			point = firstSphere.myCenterPosition + (firstSphere.myCenterPosition-secondSphere.myCenterPosition).normalized * firstSphere.myRadius;
			return true;
		}
		return false;
	}
	public static bool SwepthCircleVsPoint(SwepthCircle aSwepthSphere, Vector2 targetPoint, out Vector2 point)
	{
		point = Vector2.zero;
		Vector2 closestPoint = Vector2.zero, secondClosestPoint = Vector2.zero;
		LineSegment2D firstLine = new LineSegment2D(aSwepthSphere.myFirstPoint, aSwepthSphere.mySecondPoint);
		closestPoint = GetClosestPoint(aSwepthSphere.myFirstPoint, aSwepthSphere.mySecondPoint, targetPoint);

		if((closestPoint - targetPoint).magnitude < aSwepthSphere.myRadius)
		{
			point = closestPoint;
			return true;
		}
		return false;
	}

	public static bool SwepthSphereVsAABB(SwepthSphere aSwepthSphere, AABB anAABB, out Vector3 aIntersectionPoint)
	{
		aIntersectionPoint = aSwepthSphere.myFirstPoint;
		Vector3[] corners = new Vector3[8];

		Plane3D[] planes = anAABB.GetPlanes();

		Intersection.LineSegment3D line = new LineSegment3D(aSwepthSphere.myFirstPoint, aSwepthSphere.mySecondPoint);
		float closestDist = 999999999.0f;
		Vector3 closestPoint = Vector3.zero;
		bool foundIntersection = false;
		for(int index = 0; index < planes.Length; index++){
			if(Intersection.LineVsPlane(ref line, planes[index], out aIntersectionPoint))
			{
				float dist = (aIntersectionPoint - aSwepthSphere.myFirstPoint).sqrMagnitude;
				if(dist < closestDist){
					closestDist = dist;
					closestPoint = aIntersectionPoint;
				}
				foundIntersection = true;
				Intersection.Sphere tempSphere = new Sphere(aIntersectionPoint, aSwepthSphere.myRadius);
				if(SphereVsAABB(tempSphere, anAABB, out aIntersectionPoint) == true)
				{
					return true;
				}
			}
		}
		aIntersectionPoint = closestPoint;
		return false;
	}
	public static bool SphereVsFrustrum(Sphere aSphere, Fov90Frustrum aFov90Frustrum)
	{
		Debug.LogError("SphereVsFrustrum has not been fully implemented");
		//if(aSphere.myCenterPosition.z+aSphere.myRadius < aFov90Frustrum.myNearPlane)
		//{
		//	return false;
		//}
		//if(aSphere.myCenterPosition.z-aSphere.myRadius > aFov90Frustrum.myFarPlane)
		//{
		//	return false;
		//}
		//
		//Vector3 aTempVector;
		//
		//Vector3 aFarPlaneTop;
		//Vector3 aFarPlaneBotom;
		//Vector3 aFarPlaneRight;
		//Vector3 aFarPlaneLeft;
		//
		//
		//aFarPlaneTop.z = aFov90Frustrum.myFarPlane;
		//aFarPlaneTop *= Matrix33<float>::CreateRotateAroundX(0.25f);
		//aFarPlaneTop.z = aFov90Frustrum.myFarPlane;
		//
		//aFarPlaneBotom.z = aFov90Frustrum.myFarPlane;
		//aFarPlaneBotom.y = -aFarPlaneTop.y;
		//
		//
		//aFarPlaneRight.z = aFov90Frustrum.myFarPlane;
		//aFarPlaneRight *= Matrix33<float>::CreateRotateAroundY(0.25f);
		//aFarPlaneRight.z = aFov90Frustrum.myFarPlane;
		//
		//aFarPlaneLeft.z = aFov90Frustrum.myFarPlane;
		//aFarPlaneLeft.x = -aFarPlaneRight.x;
		//
		//aTempVector = aFarPlaneTop+aFarPlaneRight;
		//if(PointInsideSphere(aSphere, aTempVector) == true)
		//{
		//	return true;
		//}
		//
		//aTempVector = aFarPlaneTop+aFarPlaneLeft;
		//if(PointInsideSphere(aSphere, aTempVector) == true)
		//{
		//	return true;
		//}
		//
		//aTempVector = aFarPlaneBotom+aFarPlaneRight;
		//if(PointInsideSphere(aSphere, aTempVector) == true)
		//{
		//	return true;
		//}
		//
		//aTempVector = aFarPlaneBotom+aFarPlaneLeft;
		//if(PointInsideSphere(aSphere, aTempVector) == true)
		//{
		//	return true;
		//}
		//
		//Vector3 aNearPlaneTop;
		//Vector3 aNearPlaneBotom;
		//Vector3 aNearPlaneRight;
		//Vector3 aNearPlaneLeft;
		//
		//
		//aNearPlaneTop.z = aFov90Frustrum.myNearPlane;
		//aNearPlaneTop *= Matrix33<float>::CreateRotateAroundX(0.25f);
		//aNearPlaneTop.z = aFov90Frustrum.myNearPlane;
		//
		//aNearPlaneBotom.z = aFov90Frustrum.myNearPlane;
		//aNearPlaneBotom.y = -aNearPlaneTop.y;
		//
		//
		//aNearPlaneRight.z = aFov90Frustrum.myNearPlane;
		//aNearPlaneRight *= Matrix33<float>::CreateRotateAroundY(0.25f);
		//aNearPlaneRight.z = aFov90Frustrum.myNearPlane;
		//
		//aNearPlaneLeft.z = aFov90Frustrum.myNearPlane;
		//aNearPlaneLeft.x = -aNearPlaneRight.x;
		//
		//
		//aTempVector = aNearPlaneTop+aNearPlaneRight;
		//if(PointInsideSphere(aSphere, aTempVector) == true)
		//{
		//	return true;
		//}
		//
		//aTempVector = aNearPlaneTop+aNearPlaneLeft;
		//if(PointInsideSphere(aSphere, aTempVector) == true)
		//{
		//	return true;
		//}
		//
		//aTempVector = aNearPlaneBotom+aNearPlaneRight;
		//if(PointInsideSphere(aSphere, aTempVector) == true)
		//{
		//	return true;
		//}
		//
		//aTempVector = aNearPlaneBotom+aNearPlaneLeft;
		//if(PointInsideSphere(aSphere, aTempVector) == true)
		//{
		//	return true;
		//}
		//
		//ThreeDPlane::Plane<float> aPlane;
		//aPlane.InitWith3Points(Vector3(1,0,0), Vector3(0,0,0), Vector3(0,0,1));
		//if(aPlane.Inside(Vector3(0,1,0)) == false)
		//{
		//	return false;
		//}
		//
		////check top
		//aPlane.InitWith3Points(Vector3(aNearPlaneRight+aNearPlaneTop), Vector3(aNearPlaneLeft+aNearPlaneTop), Vector3(aFarPlaneLeft+aNearPlaneTop));
		//if(aPlane.Inside(aSphere.myCenterPosition) == false)
		//{
		//	return false;
		//}
		//
		////check Bot
		//aPlane.InitWith3Points(Vector3(aNearPlaneLeft+aNearPlaneBotom), Vector3(aNearPlaneRight+aNearPlaneBotom), Vector3(aFarPlaneRight+aFarPlaneBotom));
		//if(aPlane.Inside(aSphere.myCenterPosition) == false)
		//{
		//	return false;
		//}
		//
		////check Right
		//aPlane.InitWith3Points(Vector3(aFarPlaneRight+aFarPlaneBotom), Vector3(aFarPlaneRight+aNearPlaneTop), Vector3(aNearPlaneRight+aNearPlaneTop));
		//if(aPlane.Inside(aSphere.myCenterPosition) == false)
		//{
		//	return false;
		//}
		//
		////check invesed left
		//aPlane.InitWith3Points(Vector3(aFarPlaneLeft+aFarPlaneBotom), Vector3(aFarPlaneLeft+aNearPlaneTop), Vector3(aNearPlaneLeft+aNearPlaneTop));
		//if(aPlane.Inside(aSphere.myCenterPosition) == true)
		//{
		//	return false;
		//}
		return true;
	}
	public static bool SphereVsSphere2(FlatSphere aSphare, FlatSphere aSecondSphare)
	{
		Vector3 aVector = aSecondSphare.myCenterPosition - aSphare.myCenterPosition;
		float aLenth =aVector.magnitude;
		if(aLenth < aSphare.myRadius+aSecondSphare.myRadius && aLenth > (-aSphare.myRadius)-aSecondSphare.myRadius)
		{
			return true;
		}
		return false;
	}
	
	public static bool TriangleVsBox(Triangle2 aTriangle, Box aBox)
	{	
		
		//if(TriangleVsPoint(aTriangle, aBox.myPosition) == true)
		//{
		//	return true;
		//}
		//if(TriangleVsPoint(aTriangle, aBox.myPosition+aBox.mySize) == true)
		//{
		//	return true;
		//}
		//if(TriangleVsPoint(aTriangle, new Vector2(aBox.myPosition.x+aBox.mySize.x, aBox.myPosition.y)) == true)
		//{
		//	return true;
		//}
		//if(TriangleVsPoint(aTriangle, new Vector2(aBox.myPosition.x, aBox.myPosition.y+aBox.mySize.x)) == true)
		//{
		//	return true;
		//}
		//
		//
		//
		//
		//LineSegment3D aLine = new LineSegment3D();
		//aLine.myEndPos = aTriangle.myLeftSide;
		//Vector2 aInterSection = Vector2.zero;
		//AABB aAABB = new AABB();
		//aAABB.myCenterPos = aBox.myPosition + aBox.mySize/2;
		//aAABB.myMinPos = aBox.myPosition;
		//aAABB.myMaxPos = aBox.myPosition + aBox.mySize;
		//
		//if(LineVsAABB(ref aLine, ref aAABB, ref aInterSection) == true)
		//{
		//	return true;
		//}
		//aLine.myEndPos = aTriangle.myRightSide;
		//if(LineVsAABB(ref aLine, ref aAABB, ref aInterSection) == true)
		//{
		//	return true;
		//}
		//
		//aLine.myEndPos = aTriangle.myRightSide-aTriangle.myLeftSide;
		//aAABB.myCenterPos -= aTriangle.myLeftSide;
		//aAABB.myMaxPos -= aTriangle.myLeftSide;
		//aAABB.myMinPos -= aTriangle.myLeftSide;
		//if(LineVsAABB(ref aLine, ref aAABB, ref aInterSection) == true)
		//{
		//	return true;
		//}
		//
		//if(LeftOfLine(aTriangle.myLeftSide, aBox) == false)
		//{
		//	return false;
		//}
		//if(RightOfLine(aTriangle.myRightSide, aBox) == false)
		//{
		//	return false;
		//}
		//Vector2 aDirection = Vector2.zero;
		//aDirection = (Vector2.zero - aTriangle.myLeftSide) - aTriangle.myRightSide;
		//aBox.myPosition -= aTriangle.myRightSide;
		//if(LeftOfLine(aDirection, aBox) == false)
		//{
		//	return false;
		//}
		//aBox.myPosition += aTriangle.myRightSide;
		Debug.LogError("TriangleVsBox has not been fully implemented");
		return true;
	}
	public static bool TriangleVsPoint(Triangle2 aTriangle, Vector2 aPoint)
	{
		Debug.LogError("TriangleVsPoint has not been fully implemented");
		//Sphere aSphere = new Sphere();
		//aSphere.myCenterPosition = aPoint - aTriangle.myLeftSide;
		//aSphere.myRadius = 0;
		//
		//Vector2 aLineDirection = aTriangle.myRightSide - aTriangle.myLeftSide;
		//if(RightOfLine(aLineDirection, aSphere) == false)
		//{
		//	return false;
		//}
		//if(aPoint.magnitude < 0.0f)
		//{
		//	return false;
		//}
		//Sphere aSphare = new Sphere();
		//aSphare.myCenterPosition = aPoint;
		//aSphare.myRadius = 0;
		//if(LeftOfLine(aTriangle.myLeftSide, aSphare) == false)
		//{
		//	return false;
		//}
		//if(LeftOfLine(aTriangle.myRightSide, aSphare) == false)
		//{
		//	return false;
		//}
		return true;
	}
	public static bool TriangleVsSphere(Triangle2 aTriangle, Sphere aSphare)
	{
		Debug.LogError("TriangleVsSphere has not been fully implemented");
		//if((aSphare.myCenterPosition).sqrMagnitude+aSphare.myRadius < 0.0f)
		//{
		//	return false;
		//}
		//LineSegment3D aLine = new LineSegment3D();
		//aLine.myEndPos = aTriangle.myLeftSide;
		//Vector3 aInterSectionPoint = Vector3.zero;
		//if(LineVsSphere(aLine, aSphare, aInterSectionPoint) == true)
		//{
		//	return true;
		//}
		//aLine.myEndPos = aTriangle.myRightSide;
		//if(LineVsSphere(aLine, aSphare, aInterSectionPoint) == true)
		//{
		//	return true;
		//}
		//aLine.myStartPos = aTriangle.myLeftSide;
		//aLine.myEndPos = aTriangle.myRightSide;
		//if(LineVsSphere(aLine, aSphare, aInterSectionPoint) == true)
		//{
		//	return true;
		//}
		//
		//
		//Vector2 aDirection = Vector2.zero;
		//aDirection = (Vector2.zero - aTriangle.myLeftSide) - aTriangle.myRightSide;
		//aSphare.myCenterPosition -= aTriangle.myRightSide;
		//if(LeftOfLine(aDirection, aSphare) == false)
		//{
		//	return false;
		//}
		//aSphare.myCenterPosition += aTriangle.myRightSide;
		//
		//if(LeftOfLine(aTriangle.myLeftSide, aSphare) == false)
		//{
		//	return false;
		//}
		//if(LeftOfLine(aTriangle.myRightSide, aSphare) == false)
		//{
		//	return false;
		//}
		return true;
	}
	public static bool Inside(LineSegment2D aLine, Vector2 aPosition)
	{
		if(aPosition == aLine.myEndPos || aPosition == aLine.myStartPos)
		{
			return true;
		}
		Vector2 testPoint = aPosition - aLine.myStartPos;
		testPoint /= testPoint.magnitude;
		
		Vector2 direction = aLine.myEndPos - aLine.myStartPos;
		direction /= direction.magnitude;
		
		Vector2 normal;
		normal.x = -direction.y;
		normal.y = direction.x;
		
		if(Vector2.Dot(normal, testPoint) >= 0)
		{
			return (true);
		}
		else
		{
			return (false);
		}
	}
	
	public static bool Inside(LineSegment2D aLine, Vector2 aPosition, ref Vector2 newPos)
	{
		if(aPosition == aLine.myEndPos || aPosition == aLine.myStartPos)
		{
			return true;
		}
		Vector2 testPoint = aPosition - aLine.myStartPos;
		testPoint /= testPoint.magnitude;
		
		Vector2 direction = aLine.myEndPos - aLine.myStartPos;
		direction /= direction.magnitude;
		
		Vector2 normal;
		normal.x = -direction.y;
		normal.y = direction.x;
		
		if(Vector2.Dot(normal, testPoint) >= 0)
		{
			return (true);
		}
		else
		{
			newPos = aPosition.GetClosest(aLine.myStartPos, aLine.myEndPos);
			return (false);
		}
	}

	public static bool LeftOfLine(Vector2 aLineDirection, Sphere aSphare)
	{
		Vector2 aPoint = new Vector2(aSphare.myCenterPosition.x, aSphare.myCenterPosition.z);
		Vector3 aPosition = aLineDirection;
		aPosition /= aPosition.magnitude;
		Vector3 aPosition2 = aPoint;
		aPosition2 /= aPosition2.magnitude;
		//aPosition.Cross(aPosition2);
		float aNumber = Vector3.Dot(aPosition, aPosition2);
		
		if(aNumber >= 0.0f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public static bool LeftOfLine(Vector2 aLineDirection, Box aBox)
	{
		Vector2 aPoint = aBox.myPosition+(aBox.mySize/2);
		Vector3 aPosition = aLineDirection;
		aPosition /= aPosition.magnitude;
		Vector3 aPosition2 = aPoint;
		aPosition2 /= aPosition2.magnitude;
		//aPosition.Cross(aPosition2);
		float aNumber = Vector3.Dot(aPosition, aPosition2);
		
		if(aNumber > 0)
		{
			return true;
		}
		else
		{
			Vector3 lineDir4 = aLineDirection;
			lineDir4 /= lineDir4.magnitude;
			Vector3 lineEnd4 = new Vector2(aBox.myPosition.x + aBox.mySize.x, aBox.myPosition.y);
			lineEnd4 /= lineEnd4.magnitude;
			//lineDir4.Cross(lineEnd4);
			float dotResult4 = Vector3.Dot(lineDir4, lineEnd4);
			if(dotResult4 > 0)
			{
				return true;
			}
			else
			{
				Vector3 lineDir3 = aLineDirection;
				lineDir3 /= lineDir3.magnitude;
				Vector3 lineEnd3 = new Vector2(aBox.myPosition.x - aBox.mySize.x, aBox.myPosition.y);
				lineEnd3 /= lineEnd3.magnitude;
				//lineDir3.Cross(lineEnd3);
				float dotResult3 = Vector3.Dot(lineDir3, lineEnd3);
				if(dotResult3 > 0)
				{
					return true;
				}
				else
				{
					Vector3 lineDir2 = aLineDirection;
					lineDir2 /= lineDir2.magnitude;
					Vector3 lineEnd2 = new Vector2(aBox.myPosition.x, aBox.myPosition.y - aBox.mySize.x);
					lineEnd2 /= lineEnd2.magnitude;
					//lineDir2.Cross(lineEnd2);
					float dotResult2 = Vector3.Dot(lineDir2, lineEnd2);
					if(dotResult2 > 0)
					{
						return true;
					}
					else
					{
						Vector3 lineDIr = aLineDirection;
						lineDIr /= lineDIr.magnitude;
						Vector3 lineEnd = new Vector2(aBox.myPosition.x, aBox.myPosition.y + aBox.mySize.x);
						lineEnd /= lineEnd.magnitude;
						//lineDIr.Cross(lineEnd);
						float dotResult = Vector3.Dot(lineDIr, lineEnd);
						if(dotResult > 0)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
			}
		}
	}
	public static bool RightOfLine(Vector2 aLineDirection, Sphere aSphare)
	{
		return RightOfLine(aLineDirection, aSphare.myCenterPosition.GetVector2Y());
	}
	public static bool RightOfLine(Vector2 aLineDirection, Vector2 aSphare)
	{
		Vector2 aPoint = new Vector2(aSphare.x,aSphare.y);
		aPoint = aPoint.normalized;
		//aPosition.Cross(aPosition2);
		
		Vector2 rightSide = Vector2.Perpendicular(aLineDirection);//2D version of Cross
		float aNumber = Vector2.Dot(rightSide, aPoint);

		if(aNumber >= 0.0f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public static bool RightOfLine(Vector2 aLineDirection, Box aBox)
	{
		Vector2 aPoint = aBox.myPosition+(aBox.mySize/2);
		Vector2 tempDir = Vector2.zero - aLineDirection;
		Vector3 lineDir = new Vector3(tempDir.x, 0, tempDir.y);
		lineDir /= lineDir.magnitude;
		Vector3 lineEnd = aPoint;
		lineEnd /= lineEnd.magnitude;
		//lineDir.Cross(lineEnd);
		float dotResult = Vector3.Dot(lineDir, lineEnd);
		
		if(dotResult > 0)
		{
			return true;
		}
		else
		{
			Vector3 lineDir2 = aLineDirection;
			lineDir2 /= lineDir2.magnitude;
			Vector3 lineEnd2 = new Vector3(aBox.myPosition.x + aBox.mySize.x, 0, aBox.myPosition.y);
			lineEnd2 /= lineEnd2.magnitude;
			//lineDir2.Cross(lineEnd2);
			float dotResult1 = Vector3.Dot(lineDir2, lineEnd2);
			if(dotResult1 > 0)
			{
				return true;
			}
			else
			{
				Vector3 lineDir3 = aLineDirection;
				lineDir3 /= lineDir3.magnitude;
				Vector3 lineEnd3 = new Vector3(aBox.myPosition.x - aBox.mySize.x, 0, aBox.myPosition.y);
				lineEnd3 /= lineEnd3.magnitude;
				//lineDir3.Cross(lineEnd3);
				float dotResult2 = Vector3.Dot(lineDir3, lineEnd3);
				if(dotResult2 > 0)
				{
					return true;
				}
				else
				{
					Vector3 lineDir4 = aLineDirection;
					lineDir4 /= lineDir4.magnitude;
					Vector3 lineEnd4 = new Vector3(aBox.myPosition.x, 0, aBox.myPosition.y - aBox.mySize.x);
					lineEnd4 /= lineEnd4.magnitude;
					//lineDir4.Cross(lineEnd4);
					float dotResult3 = Vector3.Dot(lineDir4, lineEnd4);
					if(dotResult3 > 0)
					{
						return true;
					}
					else
					{
						Vector3 lineDir5 = aLineDirection;
						lineDir5 /= lineDir5.magnitude;
						Vector3 lineEnd5 = new Vector3(aBox.myPosition.x, 0, aBox.myPosition.y + aBox.mySize.x);
						lineEnd5 /= lineEnd5.magnitude;
						//lineDir5.Cross(lineEnd5);
						float dotResult4 = Vector3.Dot(lineDir5, lineEnd5);
						if(dotResult4 > 0)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
			}
		}
	}

	public static bool PointVsCube(Cube cube, Vector3 aPosition)
	{
		Vector3 aTestVector = aPosition - cube.center;
		Vector3[] directions = new Vector3[]{new Vector3(1,0,0), new Vector3(-1,0,0)
			, new Vector3(0,1,0), new Vector3(0,-1,0), new Vector3(0,0,1), new Vector3(0,0,-1)};

		Vector3[] sizeModifyer = new Vector3[]{new Vector3(cube.size.x,0,0), new Vector3(-cube.size.x,0,0)
			, new Vector3(0,cube.size.y,0), new Vector3(0,-cube.size.y,0), new Vector3(0,0,cube.size.z), new Vector3(0,0,-cube.size.z)};

		for(int index = 0; index < directions.Length; index++)
		{	
			float aNumber = Vector3.Dot(directions[index], aTestVector + sizeModifyer[index]);
			if(aNumber <= 0)
				return false;
		}
		return true;
	}

	public static Vector3 GetClosetPoint(Vector3 A, Vector3 B, Vector3 P, bool segmentClamp)
	{    
		Vector3 AP;
		AP= P - A;
		Vector3 AB;
		AB= B - A;
		float ab2 = AB.x * AB.x + AB.y * AB.y; 
		float ap_ab = AP.x*AB.x + AP.y*AB.y;
		float t = ap_ab / ab2;  
		if (segmentClamp)  
		{       
			if (t < 0.0f) 
				t = 0.0f;     
			else if (t > 1.0f) 
				t = 1.0f;  
		}  
		Vector3 Closest = A + AB * t;
		return Closest; 
	}
	
	public static Vector3 GetClosestPoint(Vector3 aStartPoint, Vector3 aEndPoint, Vector3 thisPoint)
	{
		Vector3 endPoint = aEndPoint - aStartPoint;
		Vector3 direction = thisPoint - aStartPoint;
		Vector3 lineDirection = endPoint.normalized;
		float lineLength = endPoint.magnitude;
		float distance = direction.magnitude;
		float angle = Vector3.Dot(lineDirection, direction.normalized);
		if(angle <= 0)
		{
			return aStartPoint;
		}
		if(angle*distance > lineLength)
		{
			return aEndPoint;
		}
		
		Vector3 closestPoint = aStartPoint + lineDirection * angle * distance;
		
		return closestPoint;
	}
	public static Vector3 GetClosestPointInRay(Ray ray, Vector3 thisPoint)
	{
		Vector3 direction = thisPoint - ray.origin;
		float distance = direction.magnitude;
		float angle = Vector3.Dot(ray.direction, direction.normalized);
		if(angle <= 0)
		{
			return ray.origin;
		}
		
		Vector3 closestPoint = ray.origin + ray.direction * angle * distance;
		
		return closestPoint;
	}

	public static Vector2 GetClosestPoint(Vector2 aStartPoint, Vector2 aEndPoint, Vector2 thisPoint)
	{
		Vector2 endPoint = aEndPoint - aStartPoint;
		Vector2 direction = thisPoint - aStartPoint;
		Vector2 lineDirection = endPoint.normalized;
		float lineLength = endPoint.magnitude;
		float distance = direction.magnitude;
		float angle = Vector2.Dot(lineDirection, direction.normalized);
		if(angle <= 0)
		{
			return aStartPoint;
		}
		if(angle*distance > lineLength)
		{
			return aEndPoint;
		}
		
		Vector2 closestPoint = aStartPoint + lineDirection * angle * distance;
		
		return closestPoint;
	}
}
