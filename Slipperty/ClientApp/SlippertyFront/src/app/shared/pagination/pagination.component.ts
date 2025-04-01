import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent {
  @Input() totalItems: number = 0;
  @Input() pageSize: number = 10;
  @Input() currentPage: number = 1;

  @Output() pageChanged = new EventEmitter<number>();

  maxVisiblePages: number = 5; // Maximum number of pages to display

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  getVisiblePages(): number[] {
    const pages: number[] = [];
    const totalPages = this.totalPages;

    if (totalPages <= this.maxVisiblePages) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    }

    let startPage = Math.max(1, this.currentPage - Math.floor(this.maxVisiblePages / 2));
    let endPage = startPage + this.maxVisiblePages - 1;

    if (endPage > totalPages) {
      endPage = totalPages;
      startPage = Math.max(1, endPage - this.maxVisiblePages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  get showEllipsisBefore(): boolean {
    return this.getVisiblePages()[0] > 2;
  }

  get showEllipsisAfter(): boolean {
    return this.getVisiblePages()[this.getVisiblePages().length - 1] < this.totalPages - 1;
  }

  get showFirstPage(): boolean {
    return this.getVisiblePages()[0] > 1;
  }

  get showLastPage(): boolean {
    return this.getVisiblePages()[this.getVisiblePages().length - 1] < this.totalPages;
  }

  changePage(newPage: number) {
    if (newPage >= 1 && newPage <= this.totalPages) {
      this.currentPage = newPage;
      this.pageChanged.emit(this.currentPage);
    }
  }

}
