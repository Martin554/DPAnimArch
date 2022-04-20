﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimArch.Visualization.Diagrams
{
    public class DiagramPool : Singleton<DiagramPool>
    {
        public GameObject graphPrefab;
        public GameObject classPrefab;
        public GameObject associationNonePrefab;
        public GameObject associationFullPrefab;
        public GameObject associationSDPrefab;
        public GameObject associationDSPrefab;
        public GameObject dependsPrefab;
        public GameObject generalizationPrefab;
        public GameObject implementsPrefab;
        public GameObject realisationPrefab;
        public GameObject interGraphLinePrefab;
        public GameObject interGraphArrowPrefab;

        public ClassDiagram ClassDiagram;
        public ObjectDiagram ObjectDiagram;

        public List<InterGraphRelation> RelationsClassToObject = new List<InterGraphRelation>();
    }
}
