using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.EntityComponent
{
    public class BaseObject
    {
        private readonly ComponentManager _ComponentManager;
        private readonly List<Component> _Components;

        public BaseObject(ComponentManager componentManager)
        {
            _Components = new List<Component>();
            _ComponentManager = componentManager;
            componentManager.AddBaseObject(this);
        }

        public void AddComponent(Component component)
        {
            if (_Components.Contains(component))
            {
                return;
            }

            _Components.Add(component);

            Dictionary<Type, List<BaseObject>> Components = _ComponentManager.Components;
            if (!Components.ContainsKey(component.GetType()))
            {
                Components.Add(component.GetType(), new List<BaseObject>());
            }
            if (!Components[component.GetType()].Contains(this))
            {
                Components[component.GetType()].Add(this);
            }

            component.Initialize(this);
        }

        public void AddComponents(List<Component> components)
        {
            components.ForEach((component) =>
            {
                AddComponent(component);
            });
        }

        public void RemoveComponent(Component component)
        {
            _Components.Remove(component);

            Dictionary<Type, List<BaseObject>> Components = _ComponentManager.Components;
            if (Components.ContainsKey(component.GetType())) {
                Components[component.GetType()].Remove(this);
            }
        }

        public TComponentType GetComponent<TComponentType>() where TComponentType : Component
        {
            return (_Components.Find((x) => x.GetType() == typeof(TComponentType)) as TComponentType);
        }

        public List<Type> GetComponentTypes()
        {
            List<Type> output = new List<Type>();
            _Components.ForEach((component) =>
            {
                output.Add(component.GetType());
            });
            return output;
        }

        public void Update(GameTime gameTime)
        {
            _Components.ForEach((component) =>
            {
                component.Update(gameTime);
            });
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _Components.ForEach((component) =>
            {
                component.Draw(spriteBatch, gameTime);
            });
        }
    }
}
