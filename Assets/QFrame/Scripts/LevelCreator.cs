using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QPhysics;

    public class LevelCreator
    {

        private Level level;
        private string json;
        public string id {get; private set;}
        public string name {get; private set;}

        public LevelCreator(string json)
        {
            level = JSONLevel.loadLevel(json);
            id = level.id;
            name = level.name;
            this.json = json;
        }

        public Level GetLevel()
        {
            Level tempLevel = level;
            level = JSONLevel.loadLevel(json);
            return tempLevel;
        }
    }

