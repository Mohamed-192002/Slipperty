export  class  Page{
  PageNumber : number | null;
  PageSize : number | null  ;
  Filter :string | null = null;
  constructor() {
    this.PageNumber = 1;
    this.PageSize = 10;
    this.Filter = null;
  }
}
