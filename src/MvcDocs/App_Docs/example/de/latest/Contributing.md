The [official trunk](http://github.com/jagregory/fluent-nhibernate) for Fluent NHibernate is hosted on GitHub. Where possible, we prefer contributors to fork us on GitHub, push changes to their repository and send us ([jagregory](http://github.com/jagregory) and [paulbatum](http://github.com/paulbatum))  a pull request. We also accept submissions in the form of .patch files though this is certainly not preferred.

# Forking us on GitHub

1. Download, install and configure msysgit. Jason Meridth has written an excellent [introduction](http://www.lostechies.com/blogs/jason_meridth/archive/2009/06/01/git-for-windows-developers-git-series-part-1.aspx) to git for Windows developers and the first post with walk you through this process - follow it up to and including the 'Generating SSH Keys' section. 

2. [Create](https://github.com/signup/free) a GitHub account and give GitHub your public key. Again, Jason Meridth has [covered](http://www.lostechies.com/blogs/jason_meridth/archive/2009/06/04/git-for-windows-developers-git-series-part-2.aspx) this.

3. Go to the [GitHub page](http://github.com/jagregory/fluent-nhibernate) for the official trunk and hit the 'fork' button.

4. Now that you have your own fork of Fluent NHibernate, you can work on that fork by cloning it locally. This is an easy step, but GitHub still has a [guide](http://github.com/guides/getting-a-copy-of-your-github-repo) for it.

5. **IMPORTANT:** Once you've successfully created a local clone of your fork, you have to make sure you set the core.autocrlf setting to false. Do this by opening a command line inside your Fluent NHibernate directory and enter the following command:

    git config core.autocrlf false

If you fail to do this correctly, you'll find that a diff of your changes will have many whitespace/line ending differences. This can manifest itself in the form of files appearing to have changed when they haven't, and entire files appearing as changed in diffs.

6. Work with your local repository and push your changes to your GitHub fork when you are ready. Follow the linked guide (above) by Jason for more basics on how to use Git.

7. When you are ready to send us your changes, hit the 'pull request' button on the GitHub page for your FNH fork. Enter a helpful message and send it!