using Magazyn_API.Data;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Shortage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Service
{
    public class ShortageFilterService
    {
        private readonly IOrderRepository _repo;
        private readonly List<string> _projects;
        private readonly List<string> _groups;
        private readonly List<string> _devices;
        private readonly List<ShortageItem> componentItems = new List<ShortageItem>();
        public ShortageFilterService(IOrderRepository repo, List<string> projects, List<string> groups, List<string> devices)
        {
            _repo = repo;

            _projects = !isNull(projects) ? projects : new List<string> { "" };
            _groups = !isNull(groups) ? groups : new List<string>{ "" };
            _devices = !isNull(devices) ? devices : new List<string> { "" };
        }

        public List<ShortageItem> GetComponentItems()
        {

            foreach(var proj in _projects)
            {
                foreach(var gro in _groups)
                {
                    foreach(var dev in _devices)
                    {
                        componentItems.AddRange(_repo.GetComponentItemsByProjectGroupAndDevice(proj, gro, dev));
                    }
                }
            }
            componentItems.OrderBy(i => i.Component.Id);
            return componentItems;
        }

        public List<ComponentShortage> GetShortages()
        {
            var components = GetComponentItems();
            List<ComponentShortage> shortages = new();
            foreach(var c in components)
            {
                if(!shortages.Any(s => s.ComponentId == c.Component.Id))
                {
                    shortages.Add(new ComponentShortage { ComponentId = c.Component.Id, Count = c.Count, Items = new() { c } });
                } else
                {
                    foreach(var s in shortages)
                    {
                        if(s.ComponentId == c.Component.Id)
                        {
                            s.Count += c.Count;
                            s.Items.Add(c);
                        }
                    }
                }
            }
            return shortages;
        }

        private bool isNull(dynamic value)
        {
            if (value == null)
                return true;
            if (value.GetType() == typeof(List<>))
                if (value.Count == 0)
                    return true;

            return false;
        }
    }
}
