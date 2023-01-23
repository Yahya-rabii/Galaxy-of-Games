# Galaxy-of-Games E-commerce website &#128722;
*A full stack academic project using *ASP.Net core 7 with the <b>MVC</b> model* (<b>M</b>odels, <b>V</b>iews, <b>C</b>ontrollers) using an *SQL* server database. 
</br></br>
<img style="display: block; margin-left: auto; margin-right: auto; width: 100%" src="https://www.ryadel.com/wp-content/uploads/2016/02/logo-aspnetmvc.png">
</br>
## &#9881;&#65039;*Installation:
-Either install the Zip file and extract it. (Opt: using `WinRAR` or `7Zip`)</br></br>
-<b>Or</b> use the command through a <b>Terminal</b> with <b>*GIT*</b> installed in your device : ```GIT CLONE https://github.com/Yahya-rabii/Galaxy-of-Games.git ```
## &#128268;*How to run :
1-Proceed to import the project through out <b>Visual Studio</b> (Opt: double click on the *.sln* file.) </br></br>
2-Make sure you have the right Nugets packages installed (Usually the Visual Studio IDE provides the packages automatically.) </br></br>
3-Make sure to <b>DELETE</b> the `Migrations` folder located in the `Solution explorer` , as well as the `Tables` in the local Database : </br></br>
Go to : ```SQL Server Object Explorer -> the (localdb) -> Database -> "mvc_gog.Data" -> Tables -> Delete all 5 tables.``` </br></br>
4-Once done, open the *Package Manager Console*, then insert these commands in order: </br></br>
| Command | Description |
| --- | --- |
| ```> ADD-MIGRATION {migrationname}``` | Creates a new Migration for the project which applies changes to the database schema in a consistent and versioned manner. |
| ```> UPDATE-DATABASE``` | Updates the database so that it's used in the website to store Users, Games/Products, and Carts. |

</br>

5-The website should function as intended.</br>

## &#129489;&#8205;&#128187;*Softwares and Programming languages used :
-C#, HTML5, CSS3, JS. </br>
-SQL Server (with MS SQL Server Management Studio). </br>
-Visual Studio 2022. </br>



<h5>Copyright &#169;&#65039; 2023. @ɴᴏxɪᴅᴇᴜꜱ, @MatrixcsYounes </h5>

