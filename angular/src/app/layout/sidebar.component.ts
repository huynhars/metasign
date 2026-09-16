import { Component, OnInit } from '@angular/core';
import { LayoutStoreService } from '@shared/layout/layout-store.service';

@Component({
    selector: 'sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
    isSidebarOpen: boolean = false;

    menuItems: any[] = [];

    sidebarExpanded: boolean = false;

    constructor(private _layoutStore: LayoutStoreService) { }

    ngOnInit(): void {
        this._layoutStore.sidebarExpanded.subscribe((value: boolean) => {
            this.sidebarExpanded = value;
            this.isSidebarOpen = value;
        });

        this.menuItems = [
            { label: 'HomePage', route: '/app/home', icon: 'fas fa-home' },
            { label: 'About', route: '/app/about', icon: 'fas fa-info-circle' }
        ];
    }

    toggleSidebar(): void {
        this.isSidebarOpen = !this.isSidebarOpen;
        this._layoutStore.setSidebarExpanded(this.isSidebarOpen);
    }
}
