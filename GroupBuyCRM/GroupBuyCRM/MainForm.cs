using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupBuyCRM
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCreateCustomer_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要添加新客户信息吗？", "提示", mess);

            if (dr == DialogResult.OK)
            {
                using (var ctx = new GBCRM())
                {
                    var customer = new CustomersInfo();
                    customer.CustomerName = txtName.Text;
                    customer.Contact1 = txtContact1.Text;
                    customer.Contact2 = txtContact2.Text;
                    customer.DateOfBirth = dpDOB.Value;
                    customer.Address = txtAddress.Text;
                    customer.Remarks = txtRemarks.Text;

                    ctx.CustomersInfo.Add(customer);
                    int result = ctx.SaveChanges();

                    if (result == 1)
                    {
                        MessageBox.Show("添加成功！");
                        ClearFields();
                    }
                }
            }

        }

        private void ClearFields()
        {
            EmptyField(txtName);
            EmptyField(txtContact1);
            EmptyField(txtContact2);
            EmptyField(txtAddress);
            EmptyField(txtRemarks);

            dpDOB.Value = DateTime.Today;
        }

        private void EmptyField(TextBox control)
        {
            control.Text = string.Empty;
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            using (var context = new GBCRM())
            {
                //Querying with Object Services and Entity SQL
                string sqlString = "SELECT VALUE cust FROM GBCRM.CustomersInfo " +
                                    "AS cust WHERE cust.CustomerName LIKE '%" + txtNameS.Text + "%'" +
                                    " AND cust.Address LIKE '%" + txtAddressS.Text + "%'" +
                                   " AND (cust.Contact1 LIKE '%" + txtContactS.Text + "%' OR cust.Contact2 LIKE '%" + txtContactS.Text + "%') " +
                                   " AND cust.Remarks LIKE '%" + txtRemarksS.Text + "%'";

                var objctx = (context as IObjectContextAdapter).ObjectContext;

                ObjectQuery<CustomersInfo> customer = objctx.CreateQuery<CustomersInfo>(sqlString);
                dataGridView1.DataSource = customer.ToList<CustomersInfo>();
            }



            //string eSqlQuery = @"SELECT VAlUE b FROM BillingDetails AS b";
            //eSqlQuery = @"SELECT VAlUE b FROM OFTYPE(BillingDetails, Model.BankAccount) AS b";
            //eSqlQuery = @"SELECT VAlUE TREAT(b as EF6Console1.BankAccount) 
            //         FROM BillingDetails AS b 
            //         WHERE b IS OF(EF6Console1.BankAccount)";
            //ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            //ObjectQuery<BillingDetail> objectQuery = objectContext.CreateQuery<BillingDetail>(eSqlQuery);
            //List<BillingDetail> billingDetails1 = objectQuery.ToList();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            PopulateProductCategory();
            PopulateProductBrand();
            PopulateProductName();
            PopulateCustomerName();
        }

        private void btnCreateProductsCategory_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要添加新产品分类吗？", "提示", mess);

            if (dr == DialogResult.OK)
            {
                using (var ctx = new GBCRM())
                {
                    var category = new ProductsCategory();
                    category.CategoryName = txtCategoryName.Text;
                    category.Remarks = txtCategoryRemarks.Text;

                    ctx.ProductsCategory.Add(category);
                    int result = ctx.SaveChanges();

                    if (result == 1)
                    {
                        MessageBox.Show("添加成功！");
                        txtCategoryName.Text = string.Empty;
                        txtCategoryRemarks.Text = string.Empty;

                        PopulateProductCategory();
                    }
                }
            }
        }

        private void btnSearchCategory_Click(object sender, EventArgs e)
        {
            using (var context = new GBCRM())
            {
                //Querying with Object Services and Entity SQL
                string sqlString = "SELECT VALUE cat FROM GBCRM.ProductsCategory " +
                                    "AS cat WHERE cat.CategoryName LIKE '%" + txtCategoryNameS.Text + "%'" +
                                   " AND cat.Remarks LIKE '%" + txtCategoryRemarksS.Text + "%'";

                var objctx = (context as IObjectContextAdapter).ObjectContext;

                ObjectQuery<ProductsCategory> cat = objctx.CreateQuery<ProductsCategory>(sqlString);
                dataGridView2.DataSource = cat.ToList<ProductsCategory>();
            }
        }

        private void btnCreateProduct_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要添加新客户信息吗？", "提示", mess);

            if (dr == DialogResult.OK)
            {
                using (var ctx = new GBCRM())
                {
                    var product = new ProductsInfo();
                    product.ProductBrand = txtProductBrand.Text;
                    product.ProductName = txtProductName.Text;
                    product.ProductCreatedDate = dpProductCreate.Value;
                    product.Origin = txtProductAddress.Text;
                    product.Color = txtProductColor.Text;
                    product.Size = txtProductSize.Text;
                    product.Remarks = txtProductRemarks.Text;
                    //product.ProductsCategory = txtRemarks.Text;
                    int catID = int.Parse(cbxProductCat.SelectedValue.ToString());
                    ProductsCategory pc = ctx.ProductsCategory.FirstOrDefault(s => s.CategoryID == catID);
                    product.ProductsCategory = pc;

                    ctx.ProductsInfo.Add(product);
                    int result = ctx.SaveChanges();

                    if (result == 1)
                    {
                        MessageBox.Show("添加成功！");
                        ClearFields();
                    }
                }
            }
        }

        private void PopulateProductCategory()
        {
            using (var context = new GBCRM())
            {
                //Querying with Object Services and Entity SQL
                string sqlString = "SELECT VALUE cat FROM GBCRM.ProductsCategory AS cat";
                var objctx = (context as IObjectContextAdapter).ObjectContext;

                ObjectQuery<ProductsCategory> cat = objctx.CreateQuery<ProductsCategory>(sqlString);

                cbxProductCat.DataSource = cat.ToList<ProductsCategory>();
                cbxProductCat.DisplayMember = "CategoryName";
                cbxProductCat.ValueMember = "CategoryID";

                cbxProductCatS.Items.Clear();
                cbxProductCatS.DisplayMember = "Text";
                cbxProductCatS.ValueMember = "Value";

                ComboboxItem bb = new ComboboxItem();
                bb.Text = "全部";
                bb.Value = 0;

                cbxProductCatS.Items.Add(bb);

                foreach (ProductsCategory item in cat)
                {
                    ComboboxItem aa = new ComboboxItem();
                    aa.Text = item.CategoryName;
                    aa.Value = item.CategoryID;

                    cbxProductCatS.Items.Add(aa);
                }

                cbxCategoryNameO.DataSource = cat.ToList<ProductsCategory>();
                cbxCategoryNameO.DisplayMember = "CategoryName";
                cbxCategoryNameO.ValueMember = "CategoryID";
            }
        }

        private void PopulateProductBrand()
        {
            using (var context = new GBCRM())
            {
                //Querying with Object Services and Entity SQL
                string sqlString = "SELECT VALUE pinfo FROM GBCRM.ProductsInfo AS pinfo";
                var objctx = (context as IObjectContextAdapter).ObjectContext;

                ObjectQuery<ProductsInfo> cat = objctx.CreateQuery<ProductsInfo>(sqlString);

                var dataset = cat.Select(x => new { x.ProductBrand }).Distinct().ToList();

                cbxProductBrandO.DataSource = dataset;
                cbxProductBrandO.DisplayMember = "ProductBrand";
            }
        }

        private void PopulateProductName()
        {
            using (var context = new GBCRM())
            {
                //Querying with Object Services and Entity SQL
                string sqlString = "SELECT VALUE pinfo FROM GBCRM.ProductsInfo AS pinfo";
                var objctx = (context as IObjectContextAdapter).ObjectContext;

                ObjectQuery<ProductsInfo> cat = objctx.CreateQuery<ProductsInfo>(sqlString);

                cbxProductNameO.DataSource = cat.ToList<ProductsInfo>();
                cbxProductNameO.DisplayMember = "ProductName";
                cbxProductNameO.ValueMember = "ProductID";
            }
        }

        private void PopulateCustomerName()
        {
            using (var context = new GBCRM())
            {
                //Querying with Object Services and Entity SQL
                string sqlString = "SELECT VALUE cinfo FROM GBCRM.CustomersInfo AS cinfo";
                var objctx = (context as IObjectContextAdapter).ObjectContext;

                ObjectQuery<CustomersInfo> cinfo = objctx.CreateQuery<CustomersInfo>(sqlString);

                cbxCustomerO.DataSource = cinfo.ToList<CustomersInfo>();
                cbxCustomerO.DisplayMember = "CustomerName";
                cbxCustomerO.ValueMember = "CustomerID";
            }
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            using (var context = new GBCRM())
            {
                

                //Querying with Object Services and Entity SQL
                string sqlString = "SELECT product.* FROM ProductsInfoes " +
                                    "AS product WHERE product.ProductBrand LIKE '%" + txtProductBrandS.Text + "%'" +
                                    " AND product.ProductName LIKE '%" + txtProductNameS.Text + "%'" +
                                   " AND product.Origin LIKE '%" + txtProductAddressS.Text + "%'" +
                                   " AND product.Color LIKE '%" + txtProductColorS.Text + "%'" +
                                   " AND product.Size LIKE '%" + txtProductSizeS.Text + "%'" +
                                   " AND product.Remarks LIKE '%" + txtRemarksS.Text + "%'";

                int catID = 0;
                if (cbxProductCatS.SelectedItem != null)
                {
                    catID = ((ComboboxItem)cbxProductCatS.SelectedItem).Value;
                }

                var dataset = context.ProductsInfo
    .Where(x => x.ProductBrand.Contains(txtProductBrandS.Text) && x.ProductName.Contains(txtProductNameS.Text) 
    && x.Origin.Contains(txtProductAddressS.Text) && x.Color.Contains(txtProductColorS.Text)
    && x.Size.Contains(txtProductSizeS.Text) && x.Remarks.Contains(txtRemarksS.Text)
    && (x.ProductsCategory.CategoryID == catID || catID == 0)
    )
    .Select(x => new { x.ProductsCategory.CategoryName, x.ProductBrand, x.ProductName, x.Origin, x.Color, x.Size, x.Remarks }).ToList();

                //var posts = context.ProductsInfo
                //       .Where(p => p.Tags == "<sql-server>")
                //       .Select(p => p);

                //List<ProductsInfo> products = context.ProductsInfo.SqlQuery(sqlString).ToList();

                //DataTable aa = new DataTable();

                //foreach (ProductsInfo item in products)
                //{
                //    item.ProductsCategory.CategoryName
                //}
                //var objctx = (context as IObjectContextAdapter).ObjectContext;

                //ObjectQuery<ProductsInfo> customer = objctx.CreateQuery<ProductsInfo>(sqlString);
                dataGridView3.DataSource = dataset;
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            //public override string ToString()
            //{
            //    return Text;
            //}
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要添加新订单信息吗？", "提示", mess);

            if (dr == DialogResult.OK)
            {
                using (var ctx = new GBCRM())
                {
                    int customerID = int.Parse(cbxCustomerO.SelectedValue.ToString());
                    int catID = int.Parse(cbxCategoryNameO.SelectedValue.ToString());
                    int productID = int.Parse(cbxProductNameO.SelectedValue.ToString());

                    var order = new OrdersInfo();
                    order.CustomerID = customerID;
                    order.CustomerName = cbxCustomerO.SelectedText;
                    order.ProductID = productID;
                    order.ProductName = cbxProductNameO.SelectedText;
                    order.Quantity = int.Parse(txtQuantityO.Text);
                    order.OrderCreatedDate = dpOrderDateO.Value;
                    order.Remarks = txtRemarksO.Text;

                    ctx.OrdersInfo.Add(order);

                    int result = ctx.SaveChanges();

                    if (result == 1)
                    {
                        MessageBox.Show("添加成功！");
                        ClearFields();
                    }
                }
            }
        }

        private void cbxCategoryNameO_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new GBCRM())
            {
                int catID = 0;
                if (cbxCategoryNameO.Items.Count > 0)
                {
                    catID = ((GroupBuyCRM.ProductsCategory)cbxCategoryNameO.SelectedItem).CategoryID;
                }

                var dataset = context.ProductsInfo
                    .Where(x => x.ProductsCategory.CategoryID == catID)
                    .Select(x => new { x.ProductBrand }).Distinct().ToList();

                var dataset2 = context.ProductsInfo
                    .Where(x => x.ProductsCategory.CategoryID == catID)
                     .Select(x => new { x.ProductID, x.ProductName }).ToList();

                cbxProductBrandO.DataSource = dataset;
                cbxProductBrandO.DisplayMember = "ProductBrand";

                cbxProductNameO.DataSource = dataset2;
                cbxProductNameO.DisplayMember = "ProductName";
                cbxProductNameO.ValueMember = "ProductID";
            }
        }

        private void cbxProductBrandO_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new GBCRM())
            {
                string aa = cbxProductBrandO.Text;

                var dataset = context.ProductsInfo
                    .Where(x => x.ProductBrand == aa)
                    .Select(x => new { x.ProductID, x.ProductName }).ToList();

                cbxProductNameO.DataSource = dataset;
                cbxProductNameO.DisplayMember = "ProductName";
                cbxProductNameO.ValueMember = "ProductID";
            }
        }
    }
}
