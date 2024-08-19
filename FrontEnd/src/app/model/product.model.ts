import Decimal from 'decimal.js';

export interface Product {
  id: string;          
  name: string;         
  description: string;  
  price: Decimal;       
  stock: number;        
}
