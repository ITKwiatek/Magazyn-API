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
        private List<ComponentShortage> shortages = new();
        public ShortageFilterService(IOrderRepository repo, List<string> projects, List<string> groups, List<string> devices)
        {
            _repo = repo;

            _projects = !isNull(projects) ? projects : new List<string> { "" };
            _groups = !isNull(groups) ? groups : new List<string>{ "" };
            _devices = !isNull(devices) ? devices : new List<string> { "" };
        }

        public ShortageFilterService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public List<ComponentShortage> GetShortagesByOrderId(int orderId)
        {
            GetComponentItemsById(orderId);
            GroupShortages();
            return shortages;
        }

        public List<ComponentShortage> GetShortages()
        {
            GetComponentItemsByProjectGroupAndDevice();
            GroupShortages();
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

        private void GroupShortages()
        {
            foreach (var c in componentItems)
            {
                if (!shortages.Any(s => s.ComponentId == c.Component.Id))
                {
                    shortages.Add(new ComponentShortage { ComponentId = c.Component.Id, Count = c.Count, Items = new() { c } });
                }
                else
                {
                    foreach (var s in shortages)
                    {
                        if (s.ComponentId == c.Component.Id)
                        {
                            s.Count += c.Count;
                            s.Items.Add(c);
                        }
                    }
                }
            }
        }

        private List<ShortageItem> GetComponentItemsById(int orderId)
        {
            componentItems.AddRange(_repo.GetComponentItemsByOrderId(orderId));
            return componentItems;
        }

        private List<ShortageItem> GetComponentItemsByProjectGroupAndDevice()
        {
            foreach (var proj in _projects)
            {
                foreach (var gro in _groups)
                {
                    foreach (var dev in _devices)
                    {
                        componentItems.AddRange(_repo.GetComponentItemsByProjectGroupAndDevice(proj, gro, dev));
                    }
                }
            }
            //componentItems = componentItems.OrderBy(i => i.Component.Id);
            return componentItems;
        }
    }
}
