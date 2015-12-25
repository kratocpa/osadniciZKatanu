using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace osadniciZKatanu
{
    public class Player : PlayerDesc
    {
        public MaterialCollection Materials { get; private set; } // suroviny hráče
        public MaterialCollection CurrentAddedMaterials { get; private set; } // suroviny, které se tomuto hráči toto kolo přičetli
        public MaterialCollection CurrentDeletedMaterials { get; private set; }
        public ActionCardCollection ActionCards { get; private set; } // seznam nevyložených akčních karet hráče
        public ActionCardCollection LinedActionCards { get; private set; } // seznam vyložených akčních karet hráče
        public List<Vertex> Village { get; private set; } // seznam postavených vesnic hráče
        public List<Vertex> Town { get; private set; } // seznam postavenách měst hráče
        public List<Edge> Road { get; private set; } // seznam postavených cest hráče

        public Player(GameDesc.color playerColor, bool real, GameProperties gmProp)
            : base(playerColor, real, gmProp)
        {
            Village = new List<Vertex>();
            Town = new List<Vertex>();
            Road = new List<Edge>();

            ActionCards = new ActionCardCollection();
            LinedActionCards = new ActionCardCollection();
            Materials = new MaterialCollection();
            CurrentAddedMaterials = new MaterialCollection();
            CurrentDeletedMaterials = new MaterialCollection();
        }

        public void Synchronize()
        {
            MaterialsDesc = Materials;
            ActionCardsDesc = ActionCards;
            LinedActionCardsDesc = LinedActionCards;

            VillageDesc.Clear();
            foreach (Vertex curVx in Village) { VillageDesc.Add(curVx); }

            TownDesc.Clear();
            foreach (Vertex curVx in Town) { TownDesc.Add(curVx); }

            RoadDesc.Clear();
            foreach (Edge curEg in Road) { RoadDesc.Add(curEg); }
        }

        public void AddPort(GameDesc.materials materialPort)
        {
            if (!PortForMaterial.Contains(materialPort))
            {
                PortForMaterial.Add(materialPort);
            }
        }

        public void AddVillage(Vertex village)
        {
            village.SetVillage(Color);
            Village.Add(village);
            VillageRemaining--;
        }

        public void AddTown(Vertex town)
        {
            town.SetTown(Color);
            Town.Add(town);
            Village.Remove(Village.Find(x => x.Coordinate.X == town.Coordinate.X && x.Coordinate.Y == town.Coordinate.Y));
            TownRemaining--;
            VillageRemaining++;
        }

        public void AddRoad(Edge road)
        {
            road.SetRoad(Color);
            Road.Add(road);

            foreach (Vertex curVx in road.VertexNeighbors)
            {
                var to = FindFurthermostVertex(0, curVx, Road);
                LongestWayLength = Math.Max(LongestWayLength, to);
            }

            RoadRemaining--;
        }

        /// <summary>
        /// smaže ze seznamu deletedList hranu deletedEdge
        /// </summary>
        /// <param name="deletedEdge"></param>
        /// <param name="deletedList"></param>
        /// <returns>vrátí seznam bez hranuy která se měla smazat</returns>
        List<Edge> DeleteEdge(Edge deletedEdge, List<Edge> deletedList)
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
        int FindFurthermostVertex(int distance, Vertex initialVertex, List<Edge> roadList)
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
            foreach (var curEg in Road)
            {
                succes = succes || (!curEg.VertexNeighborsDesc.First().Building && !curEg.VertexNeighborsDesc.First().IsHereBuildingInNeighbour());
                succes = succes || (!curEg.VertexNeighborsDesc.Last().Building && !curEg.VertexNeighborsDesc.Last().IsHereBuildingInNeighbour());
            }
            return succes;
        }
    }
}
