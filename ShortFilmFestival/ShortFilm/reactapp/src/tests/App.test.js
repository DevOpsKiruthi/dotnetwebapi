import React from 'react';
import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import App from '../App';
import Home from '../components/Home';
import Footer from '../components/Footer';
import AddProduct from '../components/AddProduct';
import DisplayProduct from '../components/DisplayProduct';

test('renders_App_with_Header_and_Routing_Links', () => {
  render(
    <App />
  );
  
  const homeLink = screen.getByText(/Home/i);
  const addProductLink = screen.getByText(/Add Product/i);
  const displayProductLink = screen.getByText(/Display Product/i);

  expect(homeLink).toBeInTheDocument();
  expect(addProductLink).toBeInTheDocument();
  expect(displayProductLink).toBeInTheDocument();
});


test('renders_Home_component_with_Heading', () => {
  render(<Home />);
  
  const headingText = screen.getByText(/Welcome/i);
  expect(headingText).toBeInTheDocument();
});

test('renders_Home_component_div_contains_store_info', () => {
  render(<Home />);
  const storeInfoDiv = screen.getAllByText(/Our store/i)
  expect (storeInfoDiv.length).toBeGreaterThan(0);
});

test('renders_Footer_component_with_correct_text', () => {
  render(<Footer />);
  const footerElement = screen.getByText(/rights reserved/i);
  expect(footerElement).toBeInTheDocument();
});

test('renders_Add_Product_component_title_correctly', () => {
  render(<App />);
  
  const AddProductLink =  screen.getByText(/Add Product/i)
  fireEvent.click(AddProductLink); 

  const titleElement = screen.getByText(/Add Product to Store/i);
  expect(titleElement).toBeInTheDocument();
});


test('renders_Display_Product_component_title_correctly', () => {
  render(<App />);
  
  const DisplayProductLink =  screen.getByText(/Display Product/i)
  fireEvent.click(DisplayProductLink); 

  const titleElement = screen.getByText(/Products in Store/i);
  expect(titleElement).toBeInTheDocument();
});


test('renders_form_input_fields_and_labels', () => {
  render(<AddProduct />);

  const productNameLabel = screen.getByLabelText(/Product Name:/i);
  const categoryLabel = screen.getByLabelText(/Category:/i);
  const stockQuantityLabel = screen.getByLabelText(/Stock Quantity:/i);
  const priceLabel = screen.getByLabelText(/Price:/i);
  const expiryDateLabel = screen.getByLabelText(/Expiry Date:/i);

  expect(productNameLabel).toBeInTheDocument();
  expect(categoryLabel).toBeInTheDocument();
  expect(stockQuantityLabel).toBeInTheDocument();
  expect(priceLabel).toBeInTheDocument();
  expect(expiryDateLabel).toBeInTheDocument();
});

test('displays_validation_errors_with_empty_input_fields', async () => {
  render(<AddProduct />);
  
  const submitButton = screen.getByText(/Submit/i);
  fireEvent.click(submitButton);
  
  await waitFor(() => {
    expect(screen.getByText('Product Name is required')).toBeInTheDocument();
    expect(screen.getByText('Category is required')).toBeInTheDocument();
    expect(screen.getByText('Stock Quantity is required')).toBeInTheDocument();
    expect(screen.getByText('Price is required')).toBeInTheDocument();
    expect(screen.getByText('Expiry Date is required')).toBeInTheDocument();
  });
});


test('fetches_data_from_the_backend_when_the_component_mounts', async () => {
  const mockData = [
    {
      id: 1,
      productName: 'Sample Product 1',
      category: 'Electronics',
      stockQuantity: 50,
      price: 100,
      expiryDate: '2023-11-15',
    },
    // Add more mock data if needed
  ];

  jest.spyOn(global, 'fetch').mockResolvedValueOnce({
    json: jest.fn().mockResolvedValueOnce(mockData),
    ok: true,
  });

  render(<DisplayProduct />);

  await waitFor(() => {
    expect(screen.getByText('Sample Product 1')).toBeInTheDocument();
    expect(screen.getByText('Electronics')).toBeInTheDocument();
    expect(screen.getByText('50')).toBeInTheDocument();
    expect(screen.getByText('$100')).toBeInTheDocument();
    expect(screen.getByText('2023-11-15')).toBeInTheDocument();
    // You can add more assertions based on your mock data and component structure
  });

  expect(global.fetch).toHaveBeenCalledWith(expect.stringContaining('/getAllStoreitem'), {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });
});
