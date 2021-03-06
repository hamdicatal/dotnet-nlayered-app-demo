﻿using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.Business.DependencyResolvers.Ninject;
using Northwind.DataAccess.Abstract;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // IoC Container sayesinde bağımlılığımız sıfıra indi. Aşağıdaki gibi Entity Framework'e bağımlılığımız kalmadı.
            _productService = InstanceFactory.GetInstance<IProductService>();
            _categoryService = InstanceFactory.GetInstance<ICategoryService>();

            // Dependency Inversion prensibine ters düştüğü için IoC Container kullanılır.
            //_productService = new ProductManager(new EfProductDal());
            //_categoryService = new CategoryManager(new EfCategoryDal());
        }

        private IProductService _productService;
        private ICategoryService _categoryService;

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
        }

        private void LoadCategories()
        {
            cmbCategory.DataSource = _categoryService.GetAll();
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";

            cmbNewCategory.DataSource = _categoryService.GetAll();
            cmbNewCategory.DisplayMember = "CategoryName";
            cmbNewCategory.ValueMember = "CategoryId";

            cmbUpdateCategory.DataSource = _categoryService.GetAll();
            cmbUpdateCategory.DisplayMember = "CategoryName";
            cmbUpdateCategory.ValueMember = "CategoryId";
        }

        private void LoadProducts()
        {
            dgvProduct.DataSource = _productService.GetAll();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgvProduct.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(cmbCategory.SelectedValue));
            }
            catch (Exception)
            {
                
            }
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
                dgvProduct.DataSource = _productService.GetProductsByName(txtProductName.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Add(new Product
                {
                    CategoryID = Convert.ToInt32(cmbNewCategory.SelectedValue),
                    ProductName = txtNewProductName.Text,
                    QuantityPerUnit = txtNewQuantityPerUnit.Text,
                    UnitPrice = Convert.ToDecimal(txtNewUnitPrice.Text),
                    UnitsInStock = Convert.ToInt16(txtNewStockAmount.Text)
                });

                LoadProducts();
                lblLog.Text = "Added succesfully!";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        
        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUpdateProductName.Text = dgvProduct.CurrentRow.Cells["ProductName"].Value.ToString();
            cmbUpdateCategory.SelectedValue = dgvProduct.CurrentRow.Cells["CategoryID"].Value;
            txtUpdateQuantityPerUnit.Text = dgvProduct.CurrentRow.Cells["QuantityPerUnit"].Value.ToString();
            txtUpdateStockAmount.Text = dgvProduct.CurrentRow.Cells["UnitsInStock"].Value.ToString();
            txtUpdateUnitPrice.Text = dgvProduct.CurrentRow.Cells["UnitPrice"].Value.ToString();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                _productService.Update(new Product
                {
                    ProductID = Convert.ToInt32(dgvProduct.CurrentRow.Cells["ProductID"].Value),
                    CategoryID = Convert.ToInt32(cmbUpdateCategory.SelectedValue),
                    ProductName = txtUpdateProductName.Text,
                    QuantityPerUnit = txtUpdateQuantityPerUnit.Text,
                    UnitPrice = Convert.ToDecimal(txtUpdateUnitPrice.Text),
                    UnitsInStock = Convert.ToInt16(txtUpdateStockAmount.Text)
                });

                LoadProducts();
                lblLog.Text = "Updated succesfully!";
            }

            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvProduct.CurrentRow != null)
            {
                try
                {
                    _productService.Delete(new Product
                    {
                        ProductID = Convert.ToInt32(dgvProduct.CurrentRow.Cells["ProductID"].Value)
                    });

                    LoadProducts();
                    lblLog.Text = "Deleted succesfully!";
                }
                catch (Exception exception) 
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }
    }
}
