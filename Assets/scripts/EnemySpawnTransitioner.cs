using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace topdown
{
    public class EnemySpawnTransitioner : GroupTransition
    {
        [SerializeField]
        GameObject[] SpawnObjects;
        int SpawnObjectIndex = 0;
        [SerializeField] PlayableDirector RoomTransitionTimeline;
        double PreSpawnTimePos;
        double PostSpawnTimePos;
        GameObject CurrentSpawnObject;
        IAnimationInterpolate animationInterpolator;
        [SerializeField] int InterpolateTrackIndex;
        public GameObject[] SetSpawnObjects { set => SpawnObjects = value; }


        private void OnEnable()
        {
            BaseRoom.OnRoomActive += AssignSpawnedObjects;
        }        

        private void OnDisable()
        {
            BaseRoom.OnRoomActive -= AssignSpawnedObjects;
        }

        void AssignSpawnedObjects(BaseRoom room)
        {
            print("enemy spawn manager received onroomactivecallback");
            if(room is ISpawnTransitionDataProvider)
            {
                GetStartEndTime();
                ISpawnTransitionDataProvider spawnTransitionDataProvider = room as ISpawnTransitionDataProvider;
                SpawnObjects = spawnTransitionDataProvider.GetSpawnObjects;
                print("spawn objects amount is " + SpawnObjects.Length);
            }
        }

        private void Awake()
        {
            //change this for an event, spawn manager will listen to it
            //alternatively start patrol as soon as individual transitions are completed
            //RoomTransitionTimeline.stopped += (PlayableDirector) => { spawnManager.SetEnemiesToPatrol(); };
            animationInterpolator = GetComponent<IAnimationInterpolate>();
            if(animationInterpolator == null)
            {
                print("there is no animation interpolator for " + gameObject);
            }
        }

        //need to make sure spawn track is always 3
        //need to check if room is enemy spawn interface
        //the start and end time of the timeline clip
        void GetStartEndTime()
        {
            TimelineAsset timelineAsset = (TimelineAsset)RoomTransitionTimeline.playableAsset;
            TrackAsset SpawnTrack = timelineAsset.GetOutputTrack(InterpolateTrackIndex);
            print("spawn track is " + SpawnTrack.name);
            foreach (TimelineClip clip in SpawnTrack.GetClips())
            {
                PreSpawnTimePos = (float)clip.start;
                print("spawntrack start is " + PreSpawnTimePos);
                PostSpawnTimePos = (float)clip.end;
            }
        }



        public override void IterateGroup(float duration, float time)
        {
            if (SpawnObjects.Length <1 || animationInterpolator == null) return;


            if (!CurrentSpawnObject)
            {
                CurrentSpawnObject = SpawnObjects[SpawnObjectIndex];
                CurrentSpawnObject.SetActive(true);
                print("adding " + CurrentSpawnObject + " to interpolator");
                animationInterpolator.InterpolateTarget = CurrentSpawnObject;
                //may also need to assign spawn object components to an interface
            }

            animationInterpolator.Interpolate(time/duration);
                

            //print("spawn transition time is " + time);

            //enemy will have an animator time controller
            //sets the time value 
            //if(CurrentEnemy)
            //{
            //    //replace with an interface that accepts an enemy class
            //    CurrentEnemy.SpawnAnimation(time);
            //}

            //we have an spawn object ready and time has exceeded
            if (time >= duration - .2f && CurrentSpawnObject)
            {
                print("time is over duration ");
                SpawnObjectIndex++;
                if (SpawnObjectIndex < SpawnObjects.Length)
                {
                    CurrentSpawnObject = null;
                    animationInterpolator.Interpolate(1);
                    ResetIteration();
                }
                else
                {
                    //all spawn objects have been looped through
                    EndIteration();
                }
            }

        }

        void ResetIteration()
        {
            print("moving timeline behind clip");
            print("resetting iteration");
            RoomTransitionTimeline.time = PreSpawnTimePos;
        }

        void EndIteration()
        {
            RoomTransitionTimeline.time = PostSpawnTimePos + .1f;
            SpawnObjectIndex = 0;
            print("no enemy to spawn, moving timeline after clip");
        }
    }


    public interface IAnimationInterpolate
    {
        GameObject InterpolateTarget { set; }
        //a value from 0 to 1
        void Interpolate(float value);
    }
}
