import { INavData } from '@coreui/angular';

export const getNavItems = (role: string): INavData[] => {
  const allNavItems: INavData[] = [
    {
      name: 'Administración',
      title: true
    },
    {
      name: 'Pólizas Admin',
      url: '/policy/list',
      iconComponent: { name: 'cil-pencil' },
      children: [
        {
          name: 'Consultar Pólizas',
          url: '/policy/list',
          icon: 'nav-icon-bullet'
        },
        {
          name: 'Emitir',
          url: '/policy/form',
          icon: 'nav-icon-bullet'
        },
      ]
    },
    {
      name: 'Mis Pólizas',
      url: '/policy/list',
      iconComponent: { name: 'cil-description' },
      children: [
        {
          name: 'Consultar',
          url: '/policy/list',
          icon: 'nav-icon-bullet'
        }
      ]
    },
    {
      name: 'Clientes',
      url: '/client/list',
      iconComponent: { name: 'cil-cursor' },
      children: [
        {
          name: 'Consultar Clientes',
          url: '/client/list',
          icon: 'nav-icon-bullet'
        },
        {
          name: 'Nuevo Cliente',
          url: '/client/form',
          icon: 'nav-icon-bullet'
        }
      ]
    }
  ];

  // Filtra según el rol
  switch(role) {
    case 'Administrador':
      return allNavItems;
    case 'Cliente':
      return allNavItems.filter(item => item.name === 'Mis Pólizas');    
    default:
      return [];
  }
};