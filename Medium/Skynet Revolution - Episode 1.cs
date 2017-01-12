using System;
using System.Collections.Generic;
using System.Linq;


using System;
using System.Collections.Generic;
using System.Diagnostics;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    private static Graphs graphs;
    static void Main(string[] args)
    {
        LoadParameter();

        // game loop
        CutLink();

        Console.ReadLine();
    }
    private static void LoadParameter()
    {
        string[] inputs;

        var param1 = Console.ReadLine();
        Console.Error.WriteLine(param1);

        inputs = param1.Split(' ');
        var nbNodes = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
        var nbLink = int.Parse(inputs[1]); // the number of links
        var nbExit = int.Parse(inputs[2]); // the number of exit gateways

        // create nodes
        graphs = new Graphs();
        
        for (int i = 0; i < nbLink; i++)
        {
            var param2 = Console.ReadLine();
            Console.Error.WriteLine(param2);

            inputs = param2.Split(' ');
            var id1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
            var id2 = int.Parse(inputs[1]);
            graphs.AddLink(id1, id2);
        }

        for (int i = 0; i < nbExit; i++)
        {
            var param3 = Console.ReadLine();
            Console.Error.WriteLine(param3);

            int EI = int.Parse(param3); // the index of a gateway node
            graphs.Nodes[EI].AddExit();
        }

    }
    private static void CutLink()
    {
        bool exit = false;
        while (!exit)
        {
            var allExitNodes = graphs.Nodes.Values.Where(x => x.IsExit).ToList();
            var allExitCalculations = allExitNodes.ToDictionary(x => x, x => new Dijkstra(graphs, x, true));

            var SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
            Console.Error.WriteLine("Agent is on {0}", SI);

            var dijkstra = new KeyValuePair<Node, Dijkstra>();
            int remain = int.MaxValue;
            foreach (var calculate in allExitCalculations)
            {
                var count = calculate.Value.Count(SI);
                if (count > 0 && count < remain)
                {
                    dijkstra = calculate;
                    remain = count;
                }

                Console.Error.WriteLine("Exit:{0}, Way:{1}, Count:{2}", calculate.Key.Id, string.Join(",", calculate.Value.GetWays(SI)), count);
            }

            string way = string.Empty;
            if (remain == 1)
            {
                way = dijkstra.Value.GetLastWay(SI);
                Console.Error.WriteLine("Selected Exit:{0}, Ways:{1}, Count:{2}", dijkstra.Key.Id, way, remain);
            }
            else
            {
                remain = int.MaxValue;
                var remainWithoutLinkToExit = int.MaxValue;
                var links = new List<Link>();
                var compute = new Dijkstra(graphs, graphs.Nodes[SI], false);
                foreach (var node in graphs.Nodes.Values.Where(nd => !nd.IsExit))
                {
                    var currentLinks = graphs.Links.Values.Where(ln => (ln.Node1.IsExit || ln.Node2.IsExit) && (ln.Node1 == node || ln.Node2 == node)).ToList();
                    var currentRemain = compute.Count(node.Id);
                    var currentWays = compute.GetDirectWays(node.Id);
                    var currentRemainWithoutLinkToExit = CountWithoutLinkToExit(node.Id, compute);

                    var currentCheck = (currentLinks.Count > 0 && currentLinks.Count > links.Count)
                        || (currentLinks.Count > 0 && currentLinks.Count == links.Count && remainWithoutLinkToExit > currentRemainWithoutLinkToExit)
                        || (currentLinks.Count > 0 && currentLinks.Count == links.Count && remainWithoutLinkToExit == currentRemainWithoutLinkToExit && remain >= currentRemain);

                    Console.Error.WriteLine("Node:{0}, ExitLinks:{1}, Remain with no exit:{2}, Check:{3}, Remain;{4}, ways:{5}",
                        node.Id, currentLinks.Count, currentRemainWithoutLinkToExit, currentCheck, currentRemain, string.Join(",", currentWays));

                    if (currentCheck)
                    {
                        int nearestExit = 0;
                        int nearestNbExit = 0;

                        var curentExitNodes = new List<Node>();
                        foreach (var nd in allExitNodes)
                        {
                            foreach (var lnk in currentLinks)
                            {
                                if (lnk.Node1 == nd || lnk.Node2 == nd)
                                {
                                    curentExitNodes.Add(nd);
                                }
                            }
                        }

                        if (curentExitNodes.Count > 0)
                        {
                            Console.Error.WriteLine("Selected Node:{0}, ExitLinks:{1}, Remain:{2}", node.Id, curentExitNodes.Count, currentRemain);
                            foreach (var exitNode in curentExitNodes)
                            {
                                var currentExitLinks = graphs.Links.Values.Where(ln => ln.Node1 == exitNode || ln.Node2 == exitNode).ToList();
                                Console.Error.WriteLine("Selected Exit Node:{0}, Link:{1}", exitNode.Id, currentExitLinks.Count);
                                if (currentExitLinks.Count > nearestNbExit)
                                {
                                    nearestExit = exitNode.Id;
                                    nearestNbExit = currentExitLinks.Count;
                                }
                            }

                            links = currentLinks;
                            var id1 = node.Id < nearestExit ? node.Id : nearestExit;
                            var id2 = node.Id < nearestExit ? nearestExit : node.Id;
                            way = string.Format("{0} {1}", id1, id2);
                            remainWithoutLinkToExit = currentRemainWithoutLinkToExit;
                            remain = currentRemain;
                        }
                    }
                }
            }

            graphs.Links.Remove(way);
            Console.Error.WriteLine("Cut ways:{0}", way);
            Console.WriteLine(way);
        }
    }

    private static int CountWithoutLinkToExit(int id, Dijkstra compute)
    {
        var count = compute.Count(id);
        var currentWays = compute.GetDirectWays(id);

        foreach (var way in currentWays)
        {
            var links = graphs.Links.Where(lnk => lnk.Value.Node1.Id == way || lnk.Value.Node2.Id == way);
            var isExitLink = false;
            foreach (var link in links)
            {
                if (link.Value.Node1.IsExit || link.Value.Node2.IsExit)
                {
                    isExitLink = true;
                }
            }

            count = isExitLink ? count - 1 : count;
        }

        return count;
    }
}

public class Graphs
{
    public Dictionary<int, Node> Nodes { get; private set; }
    public Dictionary<string, Link> Links { get; private set; }

    public Graphs()
    {
        this.Nodes = new Dictionary<int, Node>();
        this.Links = new Dictionary<string, Link>();
    }
    public void AddLink(int id1, int id2)
    {
        var node1 = this.GetValue(id1);
        var node2 = this.GetValue(id2);

        var link = id1 < id2 ? new Link(node1, node2, 1) : new Link(node2, node1, 1);

        Link existedLink;
        if (!this.Links.TryGetValue(link.Id, out existedLink))
        {
            this.Links[link.Id] = link;
        }
    }
    private Node GetValue(int id1)
    {
        Node node1;
        if (!this.Nodes.TryGetValue(id1, out node1))
        {
            node1 = new Node(id1);
            this.Nodes[id1] = node1;
        }
        return node1;
    }
}

public class Node
{
    public int Id { get; private set; }
    public bool IsExit { get; private set; }
    public Node(int id)
    {
        this.Id = id;
    }
    public void AddExit()
    {
        this.IsExit = true;
    }
}

public class Link
{
    public string Id { get; private set; }
    public Node Node1 { get; private set; }
    public Node Node2 { get; private set; }
    public int Weight { get; private set; }
    public Link(Node node1, Node node2, int weight)
    {
        this.Id = string.Format("{0} {1}", node1.Id, node2.Id);
        this.Node1 = node1;
        this.Node2 = node2;
        this.Weight = weight;
    }
    public override string ToString()
    {
        return this.Id;
    }
}

public class Dijkstra
{
    private Graphs graphs;
    private Dictionary<int, Node> nodes;
    private List<Link> links;
    private Dictionary<int, int> distances;
    private Dictionary<int, List<int>> ways;

    public Dijkstra(Graphs graphs, Node start, bool activateExit)
    {
        // Dijkstra(G,Poids,sdeb)   
        //1 Initialisation(G,sdeb)
        //2     Q := ensemble de tous les nœuds
        //3     tant que Q n'est pas un ensemble vide
        //4       faire s1 := Trouve_min(Q)
        //5       Q := Q privé de s1
        //6       pour chaque nœud s2 voisin de s1
        //7           faire maj_distances(s1,s2)

        this.graphs = graphs;
        this.Initialisation(graphs, start, activateExit);

        var node = start;
        while (node != null)
        {
            this.CalculateDistance(node);

            this.nodes.Remove(node.Id);
            var nodewithCalculatedDistance = this.distances.Where(d => this.nodes.ContainsKey(d.Key) && d.Value != int.MaxValue).OrderBy(id => id.Value);
            var minDistance = nodewithCalculatedDistance.FirstOrDefault();
            Node nextNode;
            node = this.nodes.TryGetValue(minDistance.Key, out nextNode) ? nextNode : null;
        }
    }
    public List<int> GetWays(int id)
    {
        if (this.ways.ContainsKey(id))
        {
            return this.ways[id];
        }

        return new List<int>();
    }
    public string GetLastWay(int si)
    {
        var ids = this.GetWays(si);
        foreach (var id in ids)
        {
            var node = graphs.Nodes[id];
            if (node.IsExit)
            {
                string key = si < id
                    ? string.Format("{0} {1}", si, id)
                    : string.Format("{0} {1}", id, si);

                return key;
            }
            else
            {
                return GetLastWay(id);
            }
        }

        return string.Empty;
    }
    public List<int> GetDirectWays(int id)
    {
        var ids = this.GetWays(id);
        var distance = this.Count(id);

        if (distance == 0)
        {
            return new List<int>();
        }

        foreach (var nodeId in ids)
        {
            if (this.Count(nodeId) < distance)
            {
                var res = this.GetDirectWays(nodeId);
                if (res != null)
                {
                    res.Add(nodeId);
                    return res;
                }
            }
        }

        return null;
    }
    public int Count(int id)
    {
        if (this.distances.ContainsKey(id))
        {
            return this.distances[id];
        }

        return int.MaxValue;
    }

    private void CalculateDistance(Node node)
    {
        // Select link for given Node
        var nodeLink = new Dictionary<string, Link>();
        foreach (var link in this.links)
        {
            if (link.Node1 == node || link.Node2 == node)
            {
                var nodeIn = link.Node1 == node ? link.Node1 : link.Node2;
                var nodeOut = link.Node1 == node ? link.Node2 : link.Node1;
                if (this.nodes.ContainsKey(nodeOut.Id))
                {
                    var currentLink = new Link(nodeIn, nodeOut, link.Weight);
                    Link existedLink;
                    if (nodeLink.TryGetValue(currentLink.Id, out existedLink))
                    {
                        nodeLink[currentLink.Id] = currentLink.Weight < existedLink.Weight ? currentLink : existedLink;
                    }
                    else
                    {
                        nodeLink[currentLink.Id] = currentLink;
                    }
                }
            }
        }

        // Calculate distance for each node.
        foreach (var link in nodeLink.Values)
        {
            var nodeIn = link.Node1;
            var nodeOut = link.Node2;

            var existedDistance = this.distances[nodeOut.Id];
            var currentDistance = this.distances[nodeIn.Id] + link.Weight;
            if (existedDistance > currentDistance)
            {
                this.distances[nodeOut.Id] = currentDistance;
                this.ways[nodeOut.Id] = new List<int> { nodeIn.Id };
            }
            if (existedDistance == currentDistance)
            {
                this.ways[nodeOut.Id].Add(nodeIn.Id);
            }
        }
    }
    private void Initialisation(Graphs graphs, Node startNode, bool activateExit)
    {
        //Initialisation(G,sdeb)
        //1 pour chaque point s de G
        //2 faire d[s] := infini
        //3 d[sdeb] := 0      

        this.nodes = new Dictionary<int, Node>();
        this.links = new List<Link>();
        this.distances = new Dictionary<int, int>();
        this.ways = new Dictionary<int, List<int>>();

        // Copy Graph nodes to Dijkstra.
        foreach (var node in graphs.Nodes)
        {
            if (activateExit || (!activateExit && !node.Value.IsExit))
            {
                this.nodes.Add(node.Key, node.Value);
                this.distances.Add(node.Key, int.MaxValue);
            }
        }

        // Set Distance to initial node.
        this.distances[startNode.Id] = 0;
        this.ways[startNode.Id] = new List<int>();

        // Copy Graph links to Dijkstra.
        foreach (var link in graphs.Links)
        {
            if (activateExit || (!activateExit && !link.Value.Node1.IsExit && !link.Value.Node2.IsExit))
            {
                this.links.Add(link.Value);
            }
        }
    }
}
