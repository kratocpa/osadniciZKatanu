using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace osadniciZKatanu
{
    public class Player
    {
        public MaterialCollection CurrentAddedMaterials { get; private set; } // suroviny, které se tomuto hráči toto kolo přičetli
        public MaterialCollection CurrentDeletedMaterials { get; private set; }

        public PlayerProperties PlProp;

        public Player(Game.color playerColor, bool real, GameProperties gmProp)
        {
            PlProp = new PlayerProperties(playerColor, real, gmProp.RoadRemaining, gmProp.VillageRemaining, gmProp.TownRemaining);
            CurrentAddedMaterials = new MaterialCollection();
            CurrentDeletedMaterials = new MaterialCollection();
        }

        public void AddPort(Game.materials materialPort)
        {
            if (!PlProp.PortForMaterial.Contains(materialPort))
            {
                PlProp.PortForMaterial.Add(materialPort);
            }
        }

        public void AddVillage(Vertex village)
        {
            village.SetVillage(PlProp.Color);
            PlProp.Village.Add(village);
            PlProp.VillageRemaining--;
        }

        public void AddTown(Vertex town)
        {
            town.SetTown(PlProp.Color);
            PlProp.Town.Add(town);
            PlProp.Village.Remove(PlProp.Village.Find(x => x.Coordinate.X == town.Coordinate.X && x.Coordinate.Y == town.Coordinate.Y));
            PlProp.TownRemaining--;
            PlProp.VillageRemaining++;
        }

        public void AddRoad(Edge road)
        {
            road.SetRoad(PlProp.Color);
            PlProp.Road.Add(road);

            foreach (Vertex curVx in road.VertexNeighbors)
            {
                var to = FindFurthermostVertex(0, curVx, PlProp.Road);
                PlProp.LongestWayLength = Math.Max(PlProp.LongestWayLength, to);
            }

            PlProp.RoadRemaining--;
        }

        /// <summary>
        /// smaže ze seznamu deletedList hranu deletedEdge
        /// </summary>
        /// <param name="deletedEdge"></param>
        /// <param name="deletedList"></param>
        /// <returns>vrátí seznam bez hranuy která se měla smazat</returns>
        public static List<Edge> DeleteEdge(Edge deletedEdge, List<Edge> deletedList)
        {
            var finalList = new List<Edge>();
            foreach (var curEdge in deletedList)
            {
                if (deletedEdge != curEdge)
                {
                    finalList.Add(curEdge);
                }
            }
            return finalList;
        }

        /// <summary>
        /// Najde nejvzdálenější vrchol vrcholu initialVertex jenom použitím hran se seznamu roadList
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="initialVertex"></param>
        /// <param name="roadList"></param>
        /// <returns> první hodnota dvojice je nejvzdálenější vrchol, druhý hodnota je dílka najité cesty </returns>
        public static int FindFurthermostVertex(int distance, Vertex initialVertex, List<Edge> roadList)
        {
            List<Vertex> succesor = new List<Vertex>();
            List<int> furthermostVertices = new List<int>();
            foreach (var curEdge in roadList)
            {
                if (curEdge.VertexNeighbors.First() == initialVertex)
                {
                    var furthermostVertex1 = FindFurthermostVertex(distance + 1, curEdge.VertexNeighbors.Last(), DeleteEdge(curEdge, roadList));

                    furthermostVertices.Add(furthermostVertex1);
                }
                else if (curEdge.VertexNeighbors.Last() == initialVertex)
                {
                    var furthermostVertex2 = FindFurthermostVertex(distance + 1, curEdge.VertexNeighbors.First(), DeleteEdge(curEdge, roadList));
                    furthermostVertices.Add(furthermostVertex2);
                }
            }

            int max = 0;
            int furthermostVertex = distance;
            foreach (var curTuple in furthermostVertices)
            {
                if (curTuple > max)
                {
                    furthermostVertex = curTuple;
                    max = curTuple;
                }
            }
            return furthermostVertex;
        }

        public bool IsThereFreeSpaceForVillage()
        {
            bool succes = false;
            foreach (var curEg in PlProp.Road)
            {
                succes = succes || (!curEg.VertexNeighborsDesc.First().Building && !curEg.VertexNeighborsDesc.First().IsHereBuildingInNeighbour());
                succes = succes || (!curEg.VertexNeighborsDesc.Last().Building && !curEg.VertexNeighborsDesc.Last().IsHereBuildingInNeighbour());
            }
            return succes;
        }
    }
}
